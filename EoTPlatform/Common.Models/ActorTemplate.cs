using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

    [DataContract]
    public class ActorTemplate
    {
        public ActorTemplate(string id)
        {
            this.Id = id;
            Metadata = new Dictionary<string, string>();
            Properties = new Dictionary<string, ActorTemplateProperty>();
            Commands = new List<string>();
        }

        [DataMember]
        public string Id { get; private set; }

        [DataMember]
        public IDictionary<string, string> Metadata { get; set; }

        [DataMember]
        public IDictionary<string, ActorTemplateProperty> Properties { get; set; }

        [DataMember]
        public IList<string> Commands { get; set; }
    }
}
