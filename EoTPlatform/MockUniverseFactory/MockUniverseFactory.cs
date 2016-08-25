using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace UniverseFactory.Mocks
{
    public class MockUniverseFactory : IUniverseFactory
    {
        public Task<UniverseDefinition> CreateUniverseAsync(UniverseSpecification specification)
        {
            return Task.FromResult(new UniverseDefinition());
        }
    }
}
