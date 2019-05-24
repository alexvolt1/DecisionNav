using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace RelationalInternal
{
    public class DBLocation
    {
        public void GetDATAconnectionString(DWCredentials dwc)
        {
            if (dwc.METADATAConnectionString == null || dwc.Appid == null || dwc.Bankid == null)
                throw new InvalidOperationException("Database location unavailable");
          //  using (SqlConnection conn = SecureConnection.GetSecureConnection(dwc.METADATAConnectionString ,dwc.GroupName,dwc.UserName ,dwc.Password ))
          //  {
                //Piece of the random artificial intellegence:
                string appid = dwc.Appid;
                if (dwc.Appid.StartsWith(dwc.GroupName + "_"))
                {
                    appid = dwc.Appid.Replace(dwc.GroupName + "_", "");
                }
                using (DataSet ds = GetDataSet("EXEC spGetDBLocation @BankID = '" + dwc.Bankid + "', @AppID = '" + appid + "'", dwc))
                {
                    if (ds!= null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            dwc.DBServer = row[0].ToString();
                            dwc.DBID = row[1].ToString();
                            dwc.DATAConnectionString = String.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;Connect Timeout=240", dwc.DBServer, dwc.DBID);
                        }
                    }
                    else
                    {
                        
                        throw new Exception("\r-->  spGetDBLocation did not return any data for \r Bank: "
                                              + dwc.Bankid + " \r Application: " + appid + " \r <--");
                    }
                }


           // }
          
        }

        private DataSet GetDataSet(string sql, DWCredentials dwc)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection sqlCnxn = new SqlConnection(dwc.METADATAConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, sqlCnxn))
                    {
                        cmd.CommandType = CommandType.Text;
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(ds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dwc.Exception = ex;
                return null;
            }
            return ds;
        }
    }
}