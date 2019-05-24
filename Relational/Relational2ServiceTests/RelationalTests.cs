using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RelationalInternal;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using DSS.Logs;
using System.Configuration;

namespace RelationalServiceTests
{
    [TestClass]
    public class RelationalTests
    {
        string cryptopassword = "TLRGdd?C(~u,3Jsp)";
        //string Connect = "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";
        //string Connect = "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";
        //string Connect = "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";
        //string dataconnectionString = @"Data Source=dssdb14\dev2014;Initial Catalog=BBT_PROD_Even;uid=Deciwebuser;pwd=password";
        string dataconnectionString = @"Data Source=dssdb14\dev2014;Initial Catalog=RAF PROD OA DW;Integrated Security=SSPI;Connect Timeout=240";
        string reportsql = "SELECT [Customer/Borrower Name],[Work Package #],[Work Package Name],[Request Title],[Unit Type],[Work Package Type],[Request Type],[Obligation/Account #],[Probability],[Decision Maker #1],[Decision Maker #1 Title],[Decision Maker #1 Decision],[Decision Maker #1 Decision Date],[Decision Maker #2],[Decision Maker #2 Title],[Decision Maker #2 Decision],[Decision Maker #2 Decision Date],[Decision Maker #3],[Decision Maker #3 Title],[Decision Maker #3 Decision],[Decision Maker #3 Decision Date],[Decision Maker #4],[Decision Maker #4 Title],[Decision Maker #4 Decision],[Decision Maker #4 Decision Date],[Decision Maker #5],[Decision Maker #5 Title],[Decision Maker #5 Decision],[Decision Maker #5 Decision Date],[Decision Maker #6],[Decision Maker #6 Title],[Decision Maker #6 Decision],[Decision Maker #6 Decision Date],[Decision Maker #7],[Decision Maker #7 Title],[Decision Maker #7 Decision],[Decision Maker #7 Decision Date],[Stage Code],[Stage],[STATUS],[Officer],[Requested Amount],[Booked Flag],[WP/Request Owner],[LQC GAAP],[LQC IFRS],[Collateral],[Purpose Code],[Industry],[Pricing Index],[Territory],[Region],[Office],[Obligation Type]FROM Relational_ActiveUnitQuery  WHERE  ([Date - Calendar] in ('Current Day')) ORDER BY [Customer/Borrower Name],[Work Package #],[Request Title]"; 

        public string Connect
        {
            get{
            Relational.Core.DWCredentials dwc = new Relational.Core.DWCredentials();
            dwc.DATAConnectionString = dataconnectionString;
            dwc.AuthenticationType = "integrated";
            dwc.UserName = "rryvkin_fa";
            dwc.Password = "Kirova2017";
            return dwc.BuildConnectObject(reportsql);
            }

        }
        
        public RelationalTests(){
                Log.AppendToFile = true;
                Log.Console = true;
                Log.LogPath = @"D:\Logs";
                Log.FileName = "Relational Internal Test";
        }






        [TestMethod]
        public void GetExcelReportAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int requestcount = 1;
            Log.Write("Started, reportcount :" + requestcount);
            List<Task> requests = new List<Task>();
            for (int i = 0; i < requestcount; i++)
            {
                
                Task task = Task.Factory.StartNew(() => GenerateExcelReport3());
                requests.Add(task);
            }
           Log.Write("Waiting:");
           Task.WaitAll(requests.ToArray(),600000);
           stopwatch.Stop();
           Log.Write("Done in :" + stopwatch.Elapsed );
        }


        public void GenerateExcelReport3()
        {
            using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient())
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(01,50,00);
                try
                {
                    byte[] file = client.DownloadReport2(Connect);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }



        [TestMethod]
        public void TestConnection()
        {
            using (RelationalProxy.RelationalServiceClient client = new RelationalProxy.RelationalServiceClient())
            {
                client.InnerChannel.OperationTimeout = new TimeSpan(01, 50, 00);
                try
                {
                    int c = client.TestConnection(Connect);
                  Assert.AreEqual(1, c);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

    }

}





//SELECT [Customer/Borrower Name],[Work Package #],[Work Package Name],[Request Title],[Unit Type],[Work Package Type],[Request Type],[Obligation/Account #],[Probability],[Decision Maker #1],[Decision Maker #1 Title],[Decision Maker #1 Decision],[Decision Maker #1 Decision Date],[Decision Maker #2],[Decision Maker #2 Title],[Decision Maker #2 Decision],[Decision Maker #2 Decision Date],[Decision Maker #3],[Decision Maker #3 Title],[Decision Maker #3 Decision],[Decision Maker #3 Decision Date],[Decision Maker #4],[Decision Maker #4 Title],[Decision Maker #4 Decision],[Decision Maker #4 Decision Date],[Decision Maker #5],[Decision Maker #5 Title],[Decision Maker #5 Decision],[Decision Maker #5 Decision Date],[Decision Maker #6],[Decision Maker #6 Title],[Decision Maker #6 Decision],[Decision Maker #6 Decision Date],[Decision Maker #7],[Decision Maker #7 Title],[Decision Maker #7 Decision],[Decision Maker #7 Decision Date],[Stage Code],[Stage],[STATUS],[Officer],[Requested Amount],[Booked Flag],[WP/Request Owner],[LQC GAAP],[LQC IFRS],[Collateral],[Purpose Code],[Industry],[Pricing Index],[Territory],[Region],[Office],[Obligation Type]FROM Relational_ActiveUnitQuery  WHERE  ([Date - Calendar] in ('Current Day')) ORDER BY [Customer/Borrower Name],[Work Package #],[Request Title] 
