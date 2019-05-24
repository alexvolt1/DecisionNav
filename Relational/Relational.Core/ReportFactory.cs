using System;
using System.Collections.Generic;
using log4net;

namespace Relational.Core
{
    public class ReportFactory
    {
        private static readonly ILog log = Logging.InitializeLogging(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString());
        public ReportFactory()
        {
            log.Info("Instance created");
        }
        public void GetReportDetails(IRelationalReport reportsession)
        {
            if (reportsession.SQLDataSource == null)
                            throw new ArgumentNullException("ConnectionString");
            if (reportsession.ReportId  == null)
                            throw new ArgumentNullException("ReportId");
            reportsession.OriginalReportId = reportsession.ReportId;
            reportsession.reportDetails = DATA.ReportData.GetReportDetails(reportsession);
            reportsession.NumRecs = new ReportNumRecs(reportsession);
            reportsession.ReportColumns = new ReportColumns(reportsession); 
            reportsession.ReportGroupBy = reportsession.reportDetails["ReportGroupBy"];
            reportsession.AlternateReport = reportsession.reportDetails["AlternateReport"];
            reportsession.Hierarchies = new ReportHierarchies(reportsession);
            reportsession.ReportFiltersCollection = new ReportFiltersCollection(reportsession);
            reportsession.ReportMembersCollection = new ReportMembersCollection(reportsession);
            reportsession.ReportMembersCollection.GetMemberValues();
            reportsession.ReportOrderByClause = new ReportOrderByClause(reportsession);
            reportsession.ReportGroupByClause = new ReportGroupByClause(reportsession);
            reportsession.Variance = new ReportVariance(reportsession);
            reportsession.ReportWhereClause = new ReportWhereClause(reportsession);
            
        }
        public static void ApplyUserSelection(IRelationalReport reportsession, IUserReport userreport)
        {
            if (userreport.UseAlternateReport)
            {
                reportsession.ReportId = reportsession.AlternateReport;
                reportsession.reportDetails = DATA.ReportData.GetReportDetails(reportsession);
                reportsession.NumRecs = new ReportNumRecs(reportsession);
                reportsession.ReportColumns = new ReportColumns(reportsession);
                reportsession.ReportOrderByClause = new ReportOrderByClause(reportsession);
                reportsession.ReportGroupByClause = new ReportGroupByClause(reportsession);
            }
            else
            {
                if (reportsession.ReportId == reportsession.AlternateReport)
                {
                reportsession.ReportId = reportsession.OriginalReportId;
                reportsession.reportDetails = DATA.ReportData.GetReportDetails(reportsession);
                reportsession.NumRecs = new ReportNumRecs(reportsession);
                reportsession.ReportColumns = new ReportColumns(reportsession);
                reportsession.ReportOrderByClause = new ReportOrderByClause(reportsession);
                reportsession.ReportGroupByClause = new ReportGroupByClause(reportsession);
                }

            }
        
            CONVERTERS.ToSQL.BuildDataSQL(reportsession, userreport);
        }
        public static void Sort(IRelationalReport reportsession, string col, string dir)
        {
            if (reportsession.ReportColumns.ValidColumn(col))
            {
                reportsession.ReportOrderByClause.SetSorting(reportsession, col, dir);
                reportsession.ReportGroupByClause.SetSorting(col);
                CONVERTERS.ToSQL.BuildDataSQL(reportsession);
            }
        }
        public static object GetData(RelationalReport rr, int from, int to)
        {
            return GetDataSerialized(rr, from, to);
        }
        public static object GetDataSerialized(RelationalReport rr, int from, int to)
        {
            //if (rr.NumRecs.numRecs != "")
            //{
            //    to = int.Parse(rr.NumRecs.numRecs);
            //}
              SerializedTable t = (SerializedTable)DATA.ReportData.GetDataSerialized(rr, from, to);
              t.rowscount = rr.ReportRecordsCount.ToString();
              return t;
        }
        public static byte[] GetDataForExport(RelationalReport rr)
        {
            return DATA.ReportData.GetDataForExport(rr);
        }
      
        public static List<FilterValueItem> GetReportFilterValues(ref RelationalReport rr, LevelItemExtended le, string _memSel, string _prevFilters)
        {
            return DATA.ReportData.GetReportFilterValues(rr,le,_memSel,_prevFilters);
        }
    
        public static object GetSearchResults(RelationalReport rr, UserReport Report)
        {
            return DATA.ReportData.GetSearchResults(rr, Report);
        }
        #region Relational Reports Deciweb Navigation
        public static System.Data.DataTable GetAvailableReports(DWCredentials Connect)
        {
            return DATA.ReportData.GetAvailableReportsForDatasource(Connect);
        }
        #endregion
    }
}





