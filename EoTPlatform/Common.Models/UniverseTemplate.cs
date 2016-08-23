using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class UniverseTemplate
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public List<ActorTemplate> ActorTemplates { get; set; }
    }
}
