using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RelationalInternal
{
    public class DWCredentials
    {
        string cryptopassword = "TLRGdd?C(~u,3Jsp)";
        public string TopicID { get; set; }
        public string ReportId { get; set; }
        public string Appid { get; set; }
        public string DATAConnectionString { get; set; }
        public string METADATAConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClamedUserName { get; set; }
        public object Bankid { get; set; }
        public string GroupName { get; set; }
        public Exception Exception { get; set; }
        public string DBServer { get; set; }
        public string DBID { get; set; }
        public string sql { get; set; }
        public string AuthenticationType { get; set; }
        public DWCredentials(string Connect)
        {
            string decryptedConnect = EncDec.Decrypt(Connect, cryptopassword);

            if (!string.IsNullOrEmpty(decryptedConnect))
            {
                string[] connectparams = decryptedConnect.Split(new string[] { "---" }, StringSplitOptions.None);
                foreach (string s in connectparams)
                {
                    if (s.StartsWith("quer="))
                    {
                        sql = s.Replace("quer=", "");
                    }
                    else if (s.StartsWith("meta="))
                    {
                        METADATAConnectionString = s.Replace("meta=", "");
                    }
                    else if (s.StartsWith("data="))
                    {
                        DATAConnectionString = s.Replace("data=", "");
                    }
                    else if (s.StartsWith("clmd="))
                    {
                        ClamedUserName = s.Replace("clmd=", "");
                    }
                    else if (s.StartsWith("user="))
                    {
                        UserName = s.Replace("user=", "");
                    }
                    else if (s.StartsWith("pswd="))
                    {
                        Password = s.Replace("pswd=", "");
                    }
                    else if (s.StartsWith("bank="))
                    {
                        Bankid = s.Replace("bank=", "");
                    }
                    else if (s.StartsWith("appl="))
                    {
                        Appid = s.Replace("appl=", "");
                    }
                    else if (s.StartsWith("authtype="))
                    {
                        AuthenticationType = s.Replace("authtype=", "");
                    }
                    else if (s.StartsWith("reportid="))
                    {
                        ReportId = s.Replace("reportid=", "");
                    }
                    else if (s.StartsWith("topic="))
                    {
                        TopicID = s.Replace("topic=", "");
                    }
                }
            }
            //Deprecated
            //new DBLocation().GetDATAconnectionString(this);
        }
    }
}



