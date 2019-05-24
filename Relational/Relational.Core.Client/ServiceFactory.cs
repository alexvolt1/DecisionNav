using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Relational.Core.Client.RelationalProxy;
using System.Net;
namespace Relational.Core.Client
{
    public class RelationalClient
    {
        public RelationalServiceClient GetClient(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;


            ServicePointManager.ServerCertificateValidationCallback = ((sender, cert, chain, errors) => ((HttpWebRequest)sender).Address.Host=="localhost");
            //ServicePointManager.ServerCertificateValidationCallback =((sender, certificate, chain, sslPolicyErrors) => true);
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            //binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.ReceiveTimeout = new TimeSpan(0, 5, 0);
            EndpointAddress remoteaddress = new EndpointAddress(url);
            RelationalServiceClient client = new RelationalServiceClient(binding, remoteaddress);
            return client;
        }
    }
}
