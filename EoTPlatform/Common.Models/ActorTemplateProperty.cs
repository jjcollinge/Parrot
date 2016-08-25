using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class ActorTemplateProperty
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Unit { get; set; }
        [DataMember]
        public string Value { get; set; }
    }
}
