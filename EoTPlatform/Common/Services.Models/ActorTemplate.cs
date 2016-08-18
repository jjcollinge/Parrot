using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Models
{
    public class ActorTemplate
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public List<string> Publishers { get; set; }
    }
}
