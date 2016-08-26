using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class DataEvent
    {
        [DataMember]
        public TimeSpan TimeOffset { get; set; }
        [DataMember]
        public string TargetId { get; set; }
        [DataMember]
        public string Payload { get; set; }
    }
}
