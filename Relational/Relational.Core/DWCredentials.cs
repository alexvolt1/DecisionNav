using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Relational.Core
{
    public class DWCredentials
    {
        public string TopicID { get; set; }
        public string ReportId { get; set; }

        public string DATAConnectionString { get; set; }
        public string METADATAConnectionString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClamedUserName { get; set; }
        public string ApplicationId { get; set; }
        public string SessionId { get; set; }
        public string UserGroup { get; set; }
        public string AuthenticationType { get; set; }
        public DWCredentials()
        {

        }
        public string BuildConnectObject()
        {
            return BuildConnectObject("");
        }
        public string BuildConnectObject(string sql)
        {

            if (this == null)
            {
                throw new Exception("Connection is not initialized");
            }
            string connectString = String.Format("quer={0}---meta={1}---data={2}---clmd={3}---user={4}---pswd={5}---bank={6}---appl={7}---authtype={8}---reportid={9}---topic={10}"
                , new string[]{
                     sql
                    ,METADATAConnectionString
                    ,DATAConnectionString
                    ,ClamedUserName
                    ,UserName
                    ,Password
                    ,UserGroup
                    ,ApplicationId
                    ,AuthenticationType
                    ,ReportId
                    ,TopicID
                });
            return EncDec.Encrypt(connectString, "TLRGdd?C(~u,3Jsp)");
        }


    }
}
