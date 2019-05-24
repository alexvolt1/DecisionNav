using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using DSS.Logs;

namespace RelationalInternal
{
    [ServiceContract]
     public interface IRelationalService
    {
        [OperationContract]
        byte[] DownloadReport(string sql, string Connect);

        [OperationContract]
        byte[] DownloadReport2(string Connect);

        [OperationContract]
        int TestConnection(string Connect);

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        DataSet GetDataSetInternal1(string sql, string Connect);

        [OperationContract]
        DataSet GetDataSetInternal2(string sql, string[] sqlPar, string Connect);

        [OperationContract]
        DataSet GetDataSetInternal3(string sql, List<string[]> listParameters, string Connect);

        [OperationContract]
        DataSet GetReportDataPaged(List<string[]> relReport, int from, int to, string Connect);

        [OperationContract]
        DataSet GetDataSetList(List<string[]> sqlParList);

    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    [DataContract]
    public class RelationalService : IRelationalService
    {
        Type thisType = null;
        //string ModuleLogging = ConfigurationManager.AppSettings["ModuleLogging"];
        //string InfoLogging = ConfigurationManager.AppSettings["InfoLogging"];
       // string ModuleLogging = ConfigurationManager.AppSettings["ModuleLogging"];
       // string InfoLogging = ConfigurationManager.AppSettings["InfoLogging"];
        bool isDebugging = true;
        bool isVerbose = true;
        public RelationalService() {

            //bool.TryParse(ModuleLogging, out isDebugging);
            //bool.TryParse(InfoLogging, out isVerbose);
            
            if (isDebugging)
            {
                Log.AppendToFile = true;
                Log.Console = true;
                Log.LogPath = @"C:\Logs";
                Log.FileName = "Relational Internal Service2";
                thisType = typeof(RelationalService);
                //Log.Write("Service called, debugging enabled", thisType);
            }
        }
        #region Test Methods
        public string GetDataArray(string[] arrParameters)
        {
            return arrParameters.ToString();
        }
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
            //return "sdfgh";
        }
        public DataSet GetDataSetList(List<string[]> sqlParList)
        {
           List<string[]> listResult = sqlParList.ToList<string[]>();
            DataSet ds = new DataSet();
            return ds;
        }
        #endregion
        public int TestConnection(string Connect)
        {
            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw dwc.Exception;
            }
            using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.DATAConnectionString, dwc.GroupName, dwc.UserName, dwc.Password))
            {
                try
                {
                 conn.Open();
                 return 1;
                }
                catch (Exception ex)
                {
                    Log.Write(ex.ToString(), thisType);
                    return -1;
                }
            }
        }
        public byte[] DownloadReport(string sql, string Connect)
        {
            OpenXmlPowerTools.SpreadsheetWriter02.Generator1 x = new OpenXmlPowerTools.SpreadsheetWriter02.Generator1();
            return x.GenerateExcel();
        }
        public byte[] DownloadReport2(string Connect)
        {
            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw dwc.Exception;
            }
            if (isVerbose)
            {
                Log.Write("Info Source [DownloadReport2]: " + dwc.sql, thisType);
            }
            OpenXmlPowerTools.SpreadsheetWriter.Generator2 x = new OpenXmlPowerTools.SpreadsheetWriter.Generator2(dwc);
            return x.GenerateExcel();
        }
        public DataSet GetDataSetInternal1(string sql, string Connect)
        {
            if (isVerbose)
            {
                Log.Write("Info Source [GetDataSetInternal1]: " + sql, thisType);
            }
            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw dwc.Exception;
            }
            DataSet ds = new DataSet();
            using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.DATAConnectionString, dwc.GroupName, dwc.UserName, dwc.Password))
            {
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                    {
                        adapter.Fill(ds);
                        return ds;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write("Error Source:" + sql + " " + ex.ToString(), thisType);
                    throw;
                }
            }
        }

        public DataSet GetDataSetInternal2(string sql, string[] sqlPar, string Connect)
        {
            if (isVerbose)
            {
                Log.Write("Info Source [GetDataSetInternal2]: " + sql, thisType);
            }

            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw dwc.Exception;
            }

            string parameterName = sqlPar[0];
            string parameterValue = sqlPar[1];
            SqlParameter parameter = new SqlParameter(parameterName, parameterValue);
            using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.DATAConnectionString, dwc.GroupName, dwc.UserName, dwc.Password))
            {
                DataSet ds = new DataSet();
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
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
                catch (Exception ex)
                {
                    Log.Write(ex.ToString(), thisType);
                    throw new Exception("Attempted SQL Call: <br/><br/>" + sql + "<br/><br/>| Unable to connect to database: [" + dwc.DATAConnectionString + " ]<br/><br/>" + ex.ToString());
                }
            }
        }
        public DataSet GetDataSetInternal3(string sql, List<string[]> listParameters, string Connect)
        {
            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw new Exception("Something bad happened in Relational Service, see log for details");
            }
            DataSet ds = new DataSet();

            string parameterName;
            string parameterValue;
            SqlParameter parameter;
            using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.DATAConnectionString, dwc.GroupName, dwc.UserName, dwc.Password))
            {
                try
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(sql, conn))
                        {
                            adapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                            if (listParameters != null)
                            {
                                foreach (string[] par in listParameters)
                                {
                                    parameterName = par[0];
                                    parameterValue = par[1];
                                    parameter = new SqlParameter(parameterName, parameterValue);

                                    adapter.SelectCommand.Parameters.Add(parameter);
                                }
                            }
                            adapter.Fill(ds);
                            return ds;
                        }
                }
                catch (Exception ex)
                {
                    Log.Write(ex.ToString(), thisType);
                    throw new Exception("Attempted SQL Call: <br/><br/>" + sql + "<br/><br/>| Unable to connect to database: [" + dwc.DATAConnectionString + " ]<br/><br/>" + ex.ToString());
                }
            }
        }
       
        
        //Deprecated//
        public DataSet GetReportDataPaged(List<string[]> relReport, int from, int to, string Connect)
        {
            DWCredentials dwc = new DWCredentials(Connect);
            if (dwc.Exception != null)
            {
                Log.Write(dwc.Exception.ToString(), thisType);
                throw new Exception("Something bad happened in Relational Service, see log for details");
            }
            DataSet ds = new DataSet();
            using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.DATAConnectionString, dwc.GroupName, dwc.UserName, dwc.Password))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("spGetReportData2", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            foreach (string[] par in relReport)
                            {
                                cmd.Parameters.AddWithValue(par[0], par[1]);
                            }
                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                            {
                                adapter.Fill(ds);
                            }
                        }
                }
                catch (Exception ex)
                {
                    Log.Write(ex.ToString() + "--> " + string.Join(" | ",relReport.ToArray(), thisType));
                    throw new Exception(" Unable to connect to database");
                }
                return ds;
            }
        }
    }
}
