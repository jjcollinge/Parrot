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
        public ActorTemplate()
        {
            Metadata = new Dictionary<string, string>();
            Properties = new Dictionary<string, string>();
            Commands = new List<string>();
        }

        public string Id { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public Dictionary<string, string> Properties { get; set; }
        public List<string> Commands { get; set; }
    }
}
