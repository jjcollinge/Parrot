using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    /**
     * Needs to
     *  - support 2 types of assests.
     *       1. Static i.e. Point
     *       2. Dynamic i.e. Train
     * 
     * Model connections between assets
     * Be extensible
     * Map the domain in a simple format
     * Have versions
     **/

    public class ActorTemplate
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Mobility { get; set; }
        public List<ActorRelationship> Relationships { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public Dictionary<string, string> Properties { get; set; }
    }
}
