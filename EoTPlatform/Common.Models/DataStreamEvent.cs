using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class DataStreamEvent
    {
        public DateTime DueTime { get; set; }
        public string TargetActorId { get; set; }
        public string Payload { get; set; }
    }
}
