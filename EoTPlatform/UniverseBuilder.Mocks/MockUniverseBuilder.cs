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
        public async Task<UniverseDefinition> BuildUniverseAsync(string dataSourceFilePath, UniverseTemplate template)
        {
            return new UniverseDefinition();
        }
    }
}
