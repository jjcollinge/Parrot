using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class DataStreamEvent
    {
        [DataMember]
        public DateTime DueTime { get; set; }
        [DataMember]
        public string TargetActorId { get; set; }
        [DataMember]
        public string Payload { get; set; }
    }
}
