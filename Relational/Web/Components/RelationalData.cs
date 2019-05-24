using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DeciwebProperties;
namespace Relational.Components
{
    public class RelationalData
    {

     


        internal static string connString
        {
            get
            {
                 return DeciwebProperties.ConnectionStrings.RelDataConnectionString();
            }
        }


        #region Logic

        internal static RelationalReport GetDetails(string param)
        {
            RelationalReport rr = new RelationalReport();
            rr.ReportId = param;
            rr.reportDetails= GetReportDetails(int.Parse(param));
           
            int AlternateReportID = 0;
            rr.SubtotalAllowed = rr.reportDetails["AlternateReport"] != null && int.TryParse(rr.reportDetails["AlternateReport"]
                , out AlternateReportID) && rr.reportDetails["AlternateReport"] != param;
            if (rr.SubtotalAllowed)
            {
               // rr.reportDetails = Relational.Components.RelationalData.GetReportDetails(AlternateReportID);
            }


  //         DataSet searchTypesDS = Relational.Components.RelationalData.GetSearchTypes();
            rr.Members= GetReportMembersCollection();
            rr.Filters = GetReportFilters(rr.ReportId);
        //    rr.FilterValues = GetReportFilterValues();
            rr.ReportName = rr.reportDetails["ReportName"].ToString();
            rr.ReportColWidths = rr.reportDetails["ReportColWidths"].ToString();

            return rr;
        
        }

        private static DataSet GetReportMembersCollection()
        {
            string sourceTable = string.Empty;
            string hierName = string.Empty;
            string hierId = string.Empty;
            DataSet ds = null;
            using (DataSet hierTypesDS = Relational.Components.RelationalData.GetHierTypes())
            {
                ds = new DataSet();
                foreach (DataRow row in hierTypesDS.Tables[0].Rows)
                {
                    sourceTable = row["SourceTable"].ToString();
                    hierName = row["HierName"].ToString();
                    using (DataSet dtemp = GetMembers(sourceTable))
                    {
                        if (dtemp.Tables.Count > 0)
                        {
                            DataTable dt = dtemp.Tables[0].Copy();
                            dt.TableName = hierName;
                            dt.Columns.Add("id");
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                dt.Rows[i]["id"] = i;
                            }



                            ds.Tables.Add(dt);
                        }
                    }
                }
            }
            return ds;
        }

        internal static RelationalReport GetReport(ref RelationalReport rr)
        {


            string memSel = string.Empty;
            string numRecs = string.Empty;
            string var = string.Empty;
            string extraFields = string.Empty;


            /// Report Metadata ... Continued///


            string sql = "spReportMain";
            string fieldList = rr.reportDetails["ReportDefaultColumns"];
            if (extraFields.Trim() != "")
            {
                if (rr.reportDetails["ReportTableName"].IndexOf("updown") > 0)
                {
                    fieldList = fieldList.Replace(",[Effective Date]", "," + extraFields + ",[Effective Date]");
                }

                else if (rr.reportDetails["ReportTableName"].IndexOf("NewExposure") > 0)
                {
                    fieldList = fieldList.Replace(",[Facility Exposure - End]", "," + extraFields + ",[Facility Exposure - End]");
                }

            }

            SqlParameter[] parameters = new SqlParameter[]{
            new SqlParameter("@numRecs",rr.reportDetails["ReportNumRecs"]),
            new SqlParameter("@variance",rr.reportDetails["ReportVariance"]),
            new SqlParameter("@whereStr",rr.WhereClause==null?rr.reportDetails["ReportWhereStr"]:rr.WhereClause),
            new SqlParameter("@fieldList",fieldList),
            new SqlParameter("@tableName",rr.reportDetails["ReportTableName"]),
            new SqlParameter("@order",rr.reportDetails["ReportSortColumns"]),
            new SqlParameter("@orderDir",rr.reportDetails["ReportSortDir"]),
            new SqlParameter("@groupBy",rr.reportDetails["ReportGroupBy"]),
            new SqlParameter("@memSel",memSel)
               };

            DataSet dstNewData = GetDataSet(sql, parameters, connString);
            rr.Data = dstNewData;
            return rr;
        }  


        public static int TwoPlusTwo(){
            return 4;
        }

 
        
        
        #endregion




        #region FILTER CONVERTERS

        internal static string PrevFilterToSQL(ref RelationalReport rr, List<Memsel> prevFilters)
        {
            if (prevFilters != null)
            {
                Memsel prev = null;
                List<string> sqlnode = new List<string>(); 
                List<string> sqlnodes = new List<string>(); 

                foreach (Memsel selectedmember in prevFilters)
                {
                    if (selectedmember.leaf)
                    {
                        bool samenode = sqlnode.Count==0 ||  prev.parent == selectedmember.parent;
                        if (!samenode)
                        {
                           if (sqlnode.Count == 1)
                            {
                                sqlnodes.Add(sqlnode[0]);
                                sqlnode.Clear();
                            }
                           if(sqlnode.Count>1){
                              sqlnodes.Add(string.Format("({0})",string.Join(" OR ",sqlnode.ToArray())));
                              sqlnode.Clear();
                           }
                        }
                    
                          //add to node
                           AddToNode(sqlnode, selectedmember);
                           prev = selectedmember;
                          //
                    }
                }

                if (sqlnode.Count > 0)
                {
                    sqlnodes.Add(string.Format("({0})", string.Join(" OR ", sqlnode.ToArray()))); 
                }
                return string.Join(" AND ", sqlnodes);
            }
            return "";
        }

        private static void AddToNode(List<string> sqlnode, Memsel selectedmember)
        {
            switch (selectedmember.fieldtype)
            {
                case "1":sqlnode.Add(string.Format(" [{0}] = '{1}' ", selectedmember.parent.Trim(), selectedmember.value.Trim()));
                 break;
                case "2": sqlnode.Add(string.Format(" ([{0}] BETWEEN {1} AND {2}) ", selectedmember.parent.Trim(), selectedmember.rmin ,selectedmember.rmax ));
                 break;
            }
            
        }

        #endregion

        #region USER SELECTIONS

        internal static void ApplyUserSelections(ref RelationalReport rr, UserReport userreport)
        {
            var members = userreport.SelectedMembers.ToList();
            var filters = userreport.SelectedFilters.ToList();

            string memSQL = "";
            string filtSQL = "";
            if (members.Count > 0)
            {
                memSQL = MemSelToSQL(ref rr, members);
            }
            if (filters.Count > 0)
            {
                filtSQL = PrevFilterToSQL(ref rr, filters);
            }

            string join = memSQL != "" && filtSQL != "" ? " AND " : "";
            string sql = memSQL + join + filtSQL;
            rr.WhereClause = sql;

        }

        #endregion

        #region MEMBER SELECT CONVERTERS

        internal static string MemSelToSQL(ref RelationalReport rr, List<Memsel> memsel)
        {
            int startmember = 1;
            if (memsel.Count > startmember)
            {

                string[] levelNames = rr.Members.Tables[int.Parse(memsel[startmember].hid)].Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToArray();
                TreeNode<LevelItemExtended> levels = new TreeNode<LevelItemExtended>(new LevelItemExtended(memsel[startmember].value));
                Action<TreeNode<LevelItemExtended>> action;
                foreach (Memsel selectedmember in memsel)
                {
                    int level = -1;
                    if (selectedmember.level != null && int.TryParse(selectedmember.level, out level))
                    {
                        if (level > 0)
                        {
                            LevelItemExtended item = new LevelItemExtended(selectedmember.value);
                            item.LevelName = levelNames[level - 1];
                            item.ParentName = selectedmember.parent;
                            action = delegate(TreeNode<LevelItemExtended> l)
                            {
                                AddChilds(l, item);
                            };
                            levels.Traverse(action);
                        }
                    }
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (var item in levels.Children)
                {
                    Render2(item, ref sb);
                }
                return sb.ToString();
            }

            return "";

        }


        private static void AddChilds(TreeNode<LevelItemExtended> l, LevelItemExtended item)
        {

            if (l.Value.Name == item.ParentName)
            {
                l.AddChild(item);
            }

        }

        private static void Render2(TreeNode<LevelItemExtended> item, ref System.Text.StringBuilder sb)
        {
            if (item.Parent.Children[0] != item)
            {
                sb.Append(" OR ");
            }

            sb.Append(String.Format("[{0}] = '{1}'", item.Value.LevelName, item.Value.Name));

            if (item.Children.Count > 0)
            {
                sb.Append(" AND (");
                foreach (var v in item.Children)
                {
                    Render2(v, ref sb);
                }
                sb.Append(")");
            }
        }


        #endregion


        #region ORIGINAL MEMBER SELECT WHERE CONVERTER


 


        private static void GenerateSQLWHERE(RelationalReport rr, TreeNode<LevelItemExtended> levels)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in levels.Children)
            {
                Render(item, ref sb);
            }
            string where = sb.ToString();
            rr.WhereClause = where;// != "" ? "WHERE " + where : ""; 
        }


        private static void Render(TreeNode<LevelItemExtended> item, ref System.Text.StringBuilder sb)
        {



                if (item.Parent.Children[0] != item)
                {
                    sb.Append(" OR ");
                }

                sb.Append(String.Format("[{0}] = '{1}'", item.Value.LevelName, item.Value.Name));

            

            if (item.Children.Count > 0)
            {
                sb.Append(" AND (");
                foreach (var v in item.Children)
                {
                    Render(v, ref sb);
                }
                sb.Append(")");
            }






        }


        private static void isParent(TreeNode<LevelItemExtended> l, LevelItemExtended item)
        {
            if (l.Value.Name == item.ParentName)
            {
                l.AddChild(item);
            }

        }

        #endregion


        #region Filters DATA

        // Overloads //
        internal static List<FilterValueItem> GetReportFilterValues(LevelItemExtended item)
        {
            return GetReportFilterValues(item, "", "");
        }
        internal static List<FilterValueItem> GetReportFilterValues(LevelItemExtended item, string memberSelect)
        {
            return GetReportFilterValues(item, memberSelect, "");
        }
        internal static List<FilterValueItem> GetReportFilterValues(LevelItemExtended item, string memberSelect, string prevFilter)
        {
           // List<string> values = new List<string>();
            List<FilterValueItem> values = new List<FilterValueItem>();
            string sql = "";
            if (item.listid == 0)
            {
                string currField = String.Format("[{0}]",item.Name);
                if (item.orderfield!=null && item.orderfield != "")
                {
                    currField += String.Format(",[{0}]",item.orderfield);
                }

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@tableID", string.Format("[{0}]", item.SourceTable)));
                parameters.Add(new SqlParameter("@memSel", memberSelect));
                parameters.Add(new SqlParameter("@fieldList", currField));
                parameters.Add(new SqlParameter("@prevFilter", prevFilter));

                sql = "spGetReportFilterDataFromTable";

                if (item.orderfield != null && item.orderfield != "")
                {
                    parameters.Add(new SqlParameter("@order", item.orderfield));
                }
                DataSet ds = GetDataSet(sql, parameters.ToArray(), connString);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    values.Add(new FilterValueItem(row[0].ToString()));
                }
            }
            else if (item.fieldtype == 3 || item.fieldtype == 4)
            {
              
                DataSet ds = GetDataSet("spGetReportFilterDataFromList", new SqlParameter("@LISTID", item.listid), connString);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    values.Add(new FilterValueItem(row[0].ToString()));
                }
            }
            else
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@LISTID", item.listid ));
                parameters.Add(new SqlParameter("@TABLEID", item.SourceTable));
                parameters.Add(new SqlParameter("@memSel", memberSelect));
                parameters.Add(new SqlParameter("@fieldList", "[" + item.Name + "]"));
                parameters.Add(new SqlParameter("@prevFilter", prevFilter));

                DataSet ds = GetDataSet("spGetReportFilterDataRangeFromList", parameters.ToArray(), connString);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    FilterValueItem valueitem = new FilterValueItem();
                                    valueitem.Name=row["Range"].ToString();
                                    valueitem.Value1 =row["Value1"].ToString();
                                    valueitem.Value2 =row["Value2"].ToString();

                    values.Add(new FilterValueItem(valueitem.Name,valueitem.Value1,valueitem.Value2 ));
                }
               
            }

            return values;
           
        }
        private static void GetValues(List<string> values, DataSet ds)
        {
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                values.Add(row[0].ToString());
            }
        }
        #endregion


        #region DATA Calls


        internal static Dictionary<string, string> GetReportDetails(int ReportID)
        {
            try
            {
                // ??? initialize (performs impersonation)
                string reportid = ReportID.ToString();
                Dictionary<string, string> reportDetails = new Dictionary<string, string>();

                using (SqlConnection cnxn = new SqlConnection(connString))
                {
                    try
                    {
                        cnxn.Open();
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = cnxn;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "spReportDetails";
                            cmd.Parameters.Add(new SqlParameter("@ReportID", reportid));
                            using (SqlDataReader drdReportDetails = cmd.ExecuteReader())
                            {
                                if (drdReportDetails.HasRows)
                                {
                                    while (drdReportDetails.Read())
                                    {
                                        int columns = drdReportDetails.FieldCount;
                                        for (int i = 0; i < columns; i++)
                                        {
                                            reportDetails.Add(drdReportDetails.GetName(i), drdReportDetails.GetValue(i).ToString());
                                        }

                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unable to connect to the Database" + connString);
                    }
                }
                return reportDetails;
            }
            catch (Exception e)
            {
                throw ;
            }
        }

        internal static DataSet GetMembers(string sourcetable)
        {
            return GetDataSet("Select * from [" + sourcetable + "]", connString);
        }

        internal static DataSet GetReportFilters(string ReportID)
        {
            return GetDataSet("spGetReportFilters", new SqlParameter("@ReportID", ReportID), connString);
        }

        internal static DataSet GetHierLevel(string currHierTable, string levelName, string prevSelectedSQLWHERE)
        {
            return GetDataSet("spMemSelHierarchy"
                  , new SqlParameter[] {
                    new SqlParameter("@CurrTable", currHierTable)
                   ,new SqlParameter("@CurrLevelName", levelName)
                   ,new SqlParameter("@prevSel", prevSelectedSQLWHERE)
                }

                , connString);
        }


       
        


 
       
        internal static DataSet GetSearchTypes()
        {
            return GetDataSet("EXEC spGetSearchTypes", connString);
        }

        internal static DataSet GetHierTypes()
        {
            return GetDataSet("EXEC spGetHierarchyTypes", connString);
        }

        internal static DataSet GetHierarchy(string NodeID)
        {
            return GetDataSet("spGetHierarchy2", new SqlParameter("@currHierID", NodeID), connString);
        }


        #endregion


        #region SQL DATABASE Access
       



        private static DataSet GetDataSet(string sql, SqlParameter parameter, string connString)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlCnxn = new SqlConnection(connString))
                {



                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCnxn))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (parameter != null)
                        {
                            adapter.SelectCommand.Parameters.Add(parameter);
                        }
                        adapter.Fill(ds);
                        return ds;
                    }



                }
            }
            catch (Exception ex)
            {
                if (DeciwebProperties.Configuration.isDebugging)
                {
                    throw new Exception("Attempted SQL Call: <br/><br/>" + sql + "<br/><br/>| Unable to connect to database: [" + connString + " ]<br/><br/>" + ex.ToString());
                }
                else
                {
                    throw new Exception(" Unable to connect to database");
                }

            }
        }

    
        private static DataSet GetDataSet(string sql,SqlParameter[] parameters, string connString)
        {
            DataSet ds = new DataSet();
            try
            {



                using (SqlConnection sqlCnxn = new SqlConnection(connString))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCnxn))
                    {
                        adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            foreach (SqlParameter p in parameters)
                            {
                                adapter.SelectCommand.Parameters.Add(p);
                            }
                        }
                        adapter.Fill(ds);
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                if (DeciwebProperties.Configuration.isDebugging)
                {
                    throw new Exception("Attempted SQL Call: <br/><br/>" + sql + "<br/><br/>| Unable to connect to database: [" + connString + " ]<br/><br/>" + ex.ToString());
                }
                else
                {
                    throw new Exception(" Unable to connect to database");
                }

            }
        }


        private static DataSet GetDataSet(string sql,string connString)
        {
           DataSet ds = new DataSet();
           try
           {
               using (SqlConnection sqlCnxn = new SqlConnection(connString))
               {
                   using (SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlCnxn))
                   {
                       adapter.Fill(ds);
                       return ds;
                   }
               }
           }
           catch(Exception ex)
           {
               if (DeciwebProperties.Configuration.isDebugging)
               {
                   throw new Exception("Attempted SQL Call: <br/><br/>" + sql + "<br/><br/>| Unable to connect to database: [" + connString + " ]<br/><br/>" + ex.ToString());
               }
               else
               {
                   throw new Exception(" Unable to connect to database");
               }
              
           }
        }
        
        #endregion









    }
}