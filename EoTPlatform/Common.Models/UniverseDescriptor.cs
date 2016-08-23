using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class UniverseDescriptor
    {
        public UniverseDescriptor(Dictionary<string, List<string>> serviceEndpoints)
        {
            this.ServiceEndpoints = serviceEndpoints;
            this.Id = Guid.NewGuid().ToString();
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public Dictionary<string, List<string>> ServiceEndpoints { get; private set; }
    }
}
