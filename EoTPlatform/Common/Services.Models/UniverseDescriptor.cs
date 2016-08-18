using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Models
{
    public class UniverseDescriptor
    {
        public UniverseDescriptor()
        {
            Id = Guid.NewGuid();
            ServiceEndpoints = new Dictionary<string, Uri>();
        }

        public Guid Id { get; private set; }
        public Dictionary<string, Uri> ServiceEndpoints { get; set; }
    }
}
