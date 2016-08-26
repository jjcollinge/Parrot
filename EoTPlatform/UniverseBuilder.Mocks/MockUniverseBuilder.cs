using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace UniverseBuilder.Mocks
{
    public class MockUniverseBuilder : IUniverseBuilder
    {
        public async Task<UniverseDefinition> BuildUniverseAsync(string eventStreamFilePath, UniverseTemplate template)
        {
            var def = new UniverseDefinition()
            {
                Id = template.Id,
                Status = UniverseStatus.Running
            };
            def.AddServiceEndpoints("mock", new List<string> { "localhost:8080" });
            return def;
        }
    }
}
