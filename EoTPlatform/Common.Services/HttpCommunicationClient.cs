using Microsoft.ServiceFabric.Services.Communication.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public class HttpCommunicationClient : ICommunicationClient
    {
        public HttpClient HttpClient { get; }
        public Uri Url { get; }

        public HttpCommunicationClient(HttpClient client, string address)
        {
            this.HttpClient = client;
            this.Url = new Uri(address);
        }

        public ResolvedServiceEndpoint Endpoint { get; set; }

        public string ListenerName { get; set; }

        public ResolvedServicePartition ResolvedServicePartition { get; set; }
    }
}
