using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class ActorRelationship
    {
        // List of actors to publish to
        public List<string> Consumers { get; set; }
        // List of actors to subscribe to
        public List<string> Publishers { get; set; }
        // Description of relationship
        public string Description { get; set; }
    }
}
