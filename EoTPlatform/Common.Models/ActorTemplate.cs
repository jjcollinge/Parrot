using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class ActorTemplate
    {
        public ActorTemplate(string id)
        {
            this.Id = id;
            Metadata = new Dictionary<string, string>();
            Transformations = new Dictionary<string, double>();
            Commands = new List<string>();
        }

        [DataMember]
        public string Id { get; private set; }

        [DataMember]
        public IDictionary<string, string> Metadata { get; set; }

        [DataMember]
        public IDictionary<string, double> Transformations { get; set; }

        [DataMember]
        public IList<string> Commands { get; set; }
    }
}
