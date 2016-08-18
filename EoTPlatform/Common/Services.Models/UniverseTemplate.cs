using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Models
{
    public class UniverseTemplate
    {
        public string Id { get; set; }
        public List<ActorTemplate> ActorTemplates { get; set; }
    }
}
