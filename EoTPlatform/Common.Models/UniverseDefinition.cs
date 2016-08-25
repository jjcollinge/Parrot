using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class UniverseDefinition
    {
        public UniverseDefinition()
        {
            this.ServiceEndpoints = new Dictionary<string, List<string>>();
            this.Id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Dictionary<string, List<string>> ServiceEndpoints { get; private set; }

        public void AddServiceEndpoints(string serviceName, List<string> serviceEndpoints)
        {
            this.ServiceEndpoints.Add(serviceName, serviceEndpoints);
        }
    }
}
