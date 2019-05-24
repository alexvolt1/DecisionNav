using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Relational.Core.CONVERTERS;
using DSS.Logs;
namespace Relational.Core
{
   public class ReportFiltersCollection
    {

        //string ModuleLogging = System.Configuration.ConfigurationManager.AppSettings["debugging"];
        //string InfoLogging = System.Configuration.ConfigurationManager.AppSettings["verbose"];
        //bool isDebugging = false;
        //bool isVerbose = false;
        public bool isDebugging = true;
        public bool isVerbose = true;

        private IRelationalReport reportsession;
        public List<ReportFilter> Filters= new List<ReportFilter>();
        private int _currMonth = -1;
        private int currMonth
        {
           get {
               if (_currMonth == -1)
               {
                 _currMonth = DATA.ReportData.GetCurrentMonth(reportsession);
               }
                 return _currMonth;
           }
           set { currMonth = value; }
}
           


        public ReportFiltersCollection(IRelationalReport reportsession)
        {

            //bool.TryParse(ModuleLogging, out isDebugging);
            //bool.TryParse(InfoLogging, out isVerbose);
            if (isDebugging)
            {
                Log.AppendToFile = true;
                Log.Console = true;
                Log.LogPath = @"D:\Logs";
                Log.FileName = "RelationalReportFiltersCollection";
            }
            if (isVerbose)
            {
                Log.Write("Info Logging Enabled");
            }

            this.reportsession = reportsession;
            DATA.ReportData.GetReportFilters(this,reportsession);
        }
        public object ToJson()
        {
            return (from f in Filters select new { f.FilterUID, f.Name, f.AllowAll, f.ReportStep }).ToList().OrderBy(f => f.ReportStep).GroupBy(f => f.ReportStep);
        }
        public object ToJson(Relational.Core.UserReport Report)
        {
            return (from f in Filters select new {f.FilterUID, f.Name, f.AllowAll, f.ValueItems, f.ReportStep }).ToList().OrderBy(f => f.ReportStep).GroupBy(f => f.ReportStep);
        }
        internal void ParseFilters(IUserReport userreport)
        {
            List<Memsel> selectedfilters = userreport.SelectedFilters;
            int step = userreport.Level;
            if (this.Filters != null)
            {
               var memsel = reportsession.ReportMembersCollection.ToSql();
                foreach(ReportFilter f in Filters)
                {
                    string sql;
                    List<FilterValueItem> SelectedItems = new List<FilterValueItem>();
                    //Collect Selected Items
                    if (f.ValueItems != null && f.ValueItems.Count > 0)
                    {
                        foreach (FilterValueItem item in f.ValueItems)
                        {
                            if (item.isSelected)
                                SelectedItems.Add(item);
                        }
                    }
                    if (f.ListID == 0)
                    {
                        List<string[]> parameters = new List<string[]>() { };
                        parameters.Add(new[]{"@tableID", SQLHelper.WrapSQLName( f.DataSource)});
                        parameters.Add(new[]{"@memSel", memsel});





                        if (f.OrderField != null && f.OrderField != "")
                        {
                            string orderfield = SQLHelper.WrapSQLName(f.OrderField);
                            string sourcefield = SQLHelper.WrapSQLName(f.SourceField);
                            parameters.Add(new[] { "@order", orderfield });


                            if (orderfield.Contains(sourcefield + ","))
                            {
                                orderfield = orderfield.Replace(sourcefield + ",", "");
                            }
                            else if (orderfield.Contains("," + sourcefield))
                            {
                                orderfield = orderfield.Replace("," + sourcefield, "");
                            }
                            else if (orderfield.Contains(", " + sourcefield))
                            {
                                orderfield = orderfield.Replace(", " + sourcefield, "");
                            }

                            string s = sourcefield + "," + orderfield;


                            parameters.Add(new[] { "@fieldList", s });
                        }
                        else
                        {
                            parameters.Add(new []{"@fieldList", SQLHelper.WrapSQLName( f.SourceField)});
                        }






                        parameters.Add(new[]{"@prevFilter", ""});
                        sql = "spGetReportFilterDataFromTable";

                        DataSet ds = DATA.ReportData.GetReportFilterValues(sql, parameters, reportsession.Connect);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            f.ValueItems = new List<FilterValueItem>();
                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                           f.AddValue(new FilterValueItem(row[0].ToString()));
                        }
                    }
                    else if (f.FieldType == 3 || f.FieldType == 4)
                    {
                        DataSet ds = DATA.ReportData.GetReportFilterValues("spGetReportFilterDataFromList", new[]{"@LISTID", f.ListID.ToString()}, reportsession.Connect);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            f.ValueItems = new List<FilterValueItem>();
                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            f.AddValue(new FilterValueItem(row[1].ToString()));
                        }
                    }
                    else
                    {
                        List<string[]> parameters = new List<string[]>() { };
                        parameters.Add(new[] { "@LISTID", f.ListID.ToString() });
                        parameters.Add(new[] { "@TABLEID", SQLHelper.WrapSQLName(f.DataSource) });
                        parameters.Add(new[] { "@memSel", memsel });
                        parameters.Add(new[] { "@fieldList", SQLHelper.WrapSQLName(f.SourceField) });
                        parameters.Add(new[] { "@prevFilter", "" });

                        DataSet ds = DATA.ReportData.GetReportFilterValues("spGetReportFilterDataRangeFromList", parameters, reportsession.Connect);
                        if (ds != null && ds.Tables.Count > 0)
                        {
                            f.ValueItems = new List<FilterValueItem>();
                        }
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            FilterValueItem valueitem = new FilterValueItem();
                            valueitem.Name = row["Range"].ToString();
                            valueitem.Value1 = row["Value1"].ToString();
                            valueitem.Value2 = row["Value2"].ToString();
                            f.AddValue(valueitem);
                        }
                    }

                    // Apply Previous Selections
                    if (SelectedItems.Count > 0 && f.ValueItems != null && f.ValueItems.Count > 0)
                    {
                        foreach (FilterValueItem item in f.ValueItems)
                        {
                            foreach (FilterValueItem prevselecteditem in SelectedItems)
                            {
                                if (item.Name == prevselecteditem.Name && item.Value1 == prevselecteditem.Value1 && item.Value2 == prevselecteditem.Value2)
                                    item.isSelected = true;
                            }
                        }
                    }

                    if (selectedfilters != null)
                    {
                     foreach (Memsel selectedfilter in selectedfilters)
                      {
                        ApplyClientSelectionsToFilter(selectedfilter,f);
                      }
                    }
                    if (SelectedItems.Count == 0 && f.ValueItems != null && f.ValueItems.Count > 0 && f.AllowAll == false)
                    {
                        f.ValueItems[0].isSelected = true;
                    }
                }
            }
        }
        public void ApplyClientSelectionsToFilter(Memsel selectedfilter, ReportFilter f)
        {
            if (selectedfilter.name.Length > 7)
            {
                int step = -1;
                int filterindex = -1;
                int.TryParse(selectedfilter.name.Substring(6, 1), out step);
                int.TryParse(selectedfilter.name.Substring(7, selectedfilter.name.Length - 7), out filterindex);

               // if (f.ReportStep == step + 1 && f.FilterID == filterindex + 1)
                if (selectedfilter.uid == f.FilterUID)
                {
                    if (f.ValueItems != null && f.ValueItems.Count > 0)
                    {
                        foreach (FilterValueItem item in f.ValueItems)
                        {
                            item.isSelected = false;
                        }
                    }
                    foreach (var value in selectedfilter.values)
                    {
                        var match = f.ValueItems.Where(s1 => s1.Name == value).FirstOrDefault();
                        if (match != null)
                        {
                            match.isSelected = true;
                        }
                    }
                }
            }
        }




        //public void ApplyClientSelectionsToFilter(Memsel selectedfilter, ReportFilter f)
        //{
        //    if (selectedfilter.name.Length > 7)
        //    {
        //    int step = -1;
        //    int filterindex = -1;
        //    int.TryParse(selectedfilter.name.Substring(6, 1),out step);
        //    int.TryParse(selectedfilter.name.Substring(7, selectedfilter.name.Length-7), out filterindex);

        //    if (f.ReportStep == step+1 && f.FilterID == filterindex+1)
        //    {
        //            if (f.ValueItems != null && f.ValueItems.Count > 0)
        //            {
        //                foreach (FilterValueItem item in f.ValueItems)
        //                {
        //                    item.isSelected = false;

        //                }
        //            }
           
        //            foreach (var value in selectedfilter.values)
        //            {
        //                var match = f.ValueItems.Where(s1 => s1.Name  == value).FirstOrDefault();
        //                if (match != null)
        //                {
        //                    match.isSelected  = true;
        //                }
        //            }
        //       }
        //    }
        //}
        internal string ToSql()
        {
            //int currMonth = 0;
            //currMonth = DATA.ReportData.GetCurrentMonth(reportsession);
            string val = "";
            string currUpDownRiskID = "";
            string currUpDownRiskCompID = "";
            string currUpDownSign = "";
            string ExtraFieldsVal = "";
            string otherUpDownRiskName = "";
            string currUpDownRiskName = "";
            int startMonth = 0;
            int endMonth = 0;
            List<string> nodes = new List<string>();
            if (Filters != null)
            {
                foreach (ReportFilter f in Filters)
                {
                    string name = f.Name.Trim();
                    var selected = f.ValueItems.Where(c => c.isSelected == true).ToList();
                   // var othervalues = f.ValueItems.Where(c => c.isSelected == false).ToList();
                    if (selected != null && selected.Count > 0)
                    {
                        switch (f.FieldType)
                        {
                            case 0:
                            case 1:
                                var sv = selected.Select(c => c.Name.Trim().Replace("'", "''")).ToArray();
                                string s = string.Join("','", sv);
                                nodes.Add(string.Format(" ({0} in ('{1}')) ", SQLHelper.WrapSQLName(f.SourceField),s));
                                break;
                            case 3:
//
//   Filters specific to Upgrades/Downgrades report   //
//
                                foreach (var v in selected)
                                {
                                    val = v.Name.Trim().ToUpper();
                                    if (name == "Risk Rating Type" || name == "Report Type" || name == "Time Periods")
                                    {
                                        
                                        switch (name)
                                        {
                                            case "Risk Rating Type":
                                                if (val == "FRR" || val == "LOAN GRADE"|| val == "LGD/FR"|| val == "LQC/GAAP")
                                                {
                                                    currUpDownRiskID = "M";
                                                    currUpDownRiskName = val + getUDLF();
                                                }
                                                else if (val == "ORR" || val == "PD/RR" || val == "RRR")
                                                {
                                                    currUpDownRiskID = "O";
                                                    currUpDownRiskName = val;
                                                }
                                                else if (val == "CW")
                                                {
                                                    currUpDownRiskID = "C";
                                                    currUpDownRiskName = val;
                                                }

                                                currUpDownRiskCompID = currUpDownRiskID;
                                                if (f.OrderField == "Y")
                                                    currUpDownRiskCompID += "N";
                                            break;

                                            case "Report Type":
                                                  if (val == "UPGRADES")
                                                  {
                                                   currUpDownSign = ">";
                                                   }
                                                   if (val == "DOWNGRADES")
                                                   {
                                                   currUpDownSign = "<";
                                                   }
                                            break;
                                        
                                        

                                        case "Time Periods":
                                            if (currMonth > 0)
                                            {
                                                List<SQLField> fields = new List<SQLField>();
                                                startMonth = getStartMonth(val, currMonth);
                                                endMonth = getEndMonth(val, currMonth);

                                                SQLField field;

                                               
                                                // Current 
                                                field = new SQLField();
                                                field.Name = "Current " + currUpDownRiskName;
                                                field.Expression = "[" + currUpDownRiskID + endMonth.ToString() + "]";
                                                fields.Add(field);
                                                //Log.Write(field.Name + " | " + field.Expression);

                                                //Month
                                                field = new SQLField();
                                                field.Name = "Month " + currUpDownRiskName + " Effective";
                                                string expMonthEffective = "datename(m,case ";
                                                for (int currCompMonth = endMonth - 1; currCompMonth >= startMonth; currCompMonth--)
                                                {
                                                    int currMonthNameID = currMonth - (15 - (currCompMonth + 1)); //Find the numeric month we're comparing (1-12)
                                                    if (currMonthNameID <= 0) currMonthNameID += 12; //If the search went to zero or negative, add 12
                                                    expMonthEffective += "when [" + currUpDownRiskID + endMonth.ToString() + "] <> ["
                                                                   + currUpDownRiskID + currCompMonth.ToString() + "] then '"
                                                                   + currMonthNameID.ToString() + "' ";
                                                }
                                                expMonthEffective += "end + '/1/2000')";
                                                field.Expression = expMonthEffective;
                                                fields.Add(field);
                                               // Log.Write(field.Name + " | " + field.Expression);

                                               
                                                
                                                //Previous
                                                field = new SQLField();
                                                field.Name = "Previous " + currUpDownRiskName;
                                                field.Expression = "[" + currUpDownRiskID + startMonth.ToString() + "]";
                                                fields.Add(field);
                                                //Log.Write(field.Name + " | " + field.Expression );


                                                //otherUpDownRiskName
                                 
                                                //var othervalues = f.ValueItems.Where(c => c.isSelected == false).ToList();
                                                var riskratingtypefilter = Filters.Where(x => x.Name == "Risk Rating Type").FirstOrDefault();
                                                var othervalues = riskratingtypefilter.ValueItems.Where(c => c.isSelected == false).ToList();
                                                foreach (var othervalue in othervalues)
                                                {
                                                    string suffix = "";
                                                    if (othervalue.Name.ToUpper().EndsWith("FRR") || othervalue.Name.ToUpper().EndsWith("LOAN GRADE") || othervalue.Name.ToUpper().EndsWith("LGD/FR") || othervalue.Name.ToUpper().EndsWith("LQC/GAAP"))
                                                    {
                                                        suffix = getUDLF();
                                                    }
                                                    string otherUpDownColumn = String.Format("Current {0} {1}", othervalue.Name, suffix);
                                                    field = new SQLField();
                                                    field.Name = otherUpDownColumn;
                                                    field.Expression = "[" + otherUpDownColumn + "]";
                                                    fields.Add(field);
                                                    //Log.Write(field.Name + " | " + field.Expression);
                                                }

                                                reportsession.ReportColumns.ApplyExtraFields(fields);

                                                nodes.Add(string.Format("([{0}] {1} [{2}])", currUpDownRiskCompID + startMonth.ToString()
                                                  , currUpDownSign, currUpDownRiskCompID + endMonth.ToString()));
                                            }
                                        break;
                                     }
                                    }
                                    else
                                    {
                                       nodes.Add(string.Format("({0} = '{1}')", SQLHelper.WrapSQLName(f.SourceField), selected[0].Name.Trim()));
                                    }
                                 }
                            break;
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            case 4:

                            foreach (var v in selected)
                            {
                                val = v.Name.Trim().ToUpper();
                                if (currMonth > 0)
                                {
                                    if (name == "Time Periods")
                                    {
                                        startMonth = getStartMonth(val, currMonth);
                                        endMonth = getEndMonth(val, currMonth);
                                        ExtraFieldsVal = " CASE WHEN [FACAMT" + startMonth.ToString() + "] = 0 AND [FACAMT" + endMonth.ToString()
                                                      + "] <> 0 THEN ''New'' ELSE ''Existing'' END AS [New Or Existing Facility], [FACAMT"
                                                      + endMonth.ToString() + "] As [Last Changed Exposure Amount], datename(m,case ";
                                        for (int currCompMonth = endMonth - 1; currCompMonth >= startMonth; currCompMonth--)
                                        {
                                            int currMonthNameID = currMonth - (15 - (currCompMonth + 1)); //Find the numeric month we're comparing (1-12)
                                            if (currMonthNameID <= 0) currMonthNameID += 12; //If the search went to zero or negative, add 12
                                            ExtraFieldsVal += "when [FACAMT" + endMonth.ToString() + "] <> [FACAMT" + currCompMonth.ToString() + "] then ''"
                                                + currMonthNameID.ToString() + "'' ";
                                        }
                                        ExtraFieldsVal += "end + ''/1/2000'') As [Month of Last Exposure Change], [FACAMT" + startMonth.ToString()
                                                     + "] As [Previous Exposure Amount], [FACAMT" + endMonth.ToString()
                                                     + "] - [FACAMT" + startMonth.ToString() + "] AS [Exposure Change]";
                                    }
                                }
                            }
                            break;
                            case 2:
                            List<string> filterselectednodes = new List<string>();
                                  foreach (var v in selected)
                                  {
                                     if (v.Value1 == "")
                                     {
                                         filterselectednodes.Add(string.Format("({0} <{1})", SQLHelper.WrapSQLName(f.SourceField.Trim()), v.Value2));
                                     }
                                     else if (v.Value2 == "")
                                     {
                                         filterselectednodes.Add(string.Format("({0} >= {1})", SQLHelper.WrapSQLName(f.SourceField.Trim()), v.Value1));
                                     }
                                     else
                                     {
                                         filterselectednodes.Add(string.Format("({0} BETWEEN {1} AND {2})", SQLHelper.WrapSQLName(f.SourceField.Trim()), v.Value1, v.Value2));
                                     }
                                  }
                                  nodes.Add(string.Format("({0})",string.Join(" or ", filterselectednodes.ToArray())));
                                break;
                        }
                    }
                }
            }
            return string.Join(" AND ", nodes.ToArray()); ;
        }


        private int getStartMonth(string timeText, int currMonth)
        {
            int startMonth = 14;
            //Quarter Comparisons
            if (timeText.Substring(0, 2).ToUpper() == "QU")
            {
                int monthDelta = 15 - currMonth;
                int currQ = int.Parse(timeText.Substring(timeText.Length - 1, 1));
                startMonth = ((currQ - 1) * 3) + monthDelta;
                if (startMonth >= 13)
                    startMonth -= 12;
            }
            //Year-to-Year comparison
            else if (timeText.ToUpper() == "YEAR-TO-YEAR")
                startMonth = 3;
            //Year-to-Date comparison
            else if (timeText.ToUpper() == "YEAR-TO-DATE")
                startMonth = 15 - currMonth;
            return startMonth;
        }
        private int getEndMonth(string timeText, int currMonth)
        {
            int endMonth = 15;
            //Quarter Comparisons
            if (timeText.Substring(0, 2).ToUpper() == "QU")
            {
                int monthDelta = 15 - currMonth;
                int currQ = int.Parse(timeText.Substring(timeText.Length - 1, 1));
                endMonth = ((currQ * 3)) + monthDelta;
                if (endMonth > 15)
                    endMonth -= 12;
            }
            return endMonth;
        }
        private string getUDLF()
        {
            if (reportsession.ReportId == "12")
            {
                return "(Loan)";
            }
            return "(Facility)";

        }
    }
   public class ReportFilter
   {
       private DataRow row;
       public string Name { get; set; }
       public bool AllowAll { get; set; }
       internal string DataSource { get; set; }
       public string SourceField { get; set; }
       public int ReportStep { get; set; }
       public int FilterID { get; set; }
       public string FilterUID { get; set; }
       public int ListID { get; set; }
       public string OrderField { get; set; }
       public int FieldType { get; set; }       
       public List<FilterValueItem> ValueItems { get; set; }
       public ReportFilter(DataRow row)
       {
           this.row = row;
           this.Name = row["FilterName"].ToString();
           this.AllowAll = row["AllowAll"].ToString().ToLower()=="y"?true:false;
           this.ReportStep = (int)row["ReportStep"];
           this.FilterID = (int)row["FilterID"];
           this.ListID = (int)row["ListID"];
           this.DataSource = row["DataSource"].ToString();
           this.SourceField = row["SourceField"].ToString();
           this.OrderField = row["OrderField"].ToString();
           this.FieldType = Convert.IsDBNull(row["FieldType"])?0: (int)row["FieldType"];
           this.FilterUID = UniqueID.RNGCharacterMask();
       }

       internal void AddValue(FilterValueItem filterValueItem)
       {
           ValueItems.Add(filterValueItem);
       }



       
   }
}


