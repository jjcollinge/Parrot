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
        public Task<UniverseDescriptor> CreateUniverseAsync(UniverseSpecification specification)
        {
            return Task.FromResult(new UniverseDescriptor(new Dictionary<string, List<string>>()));
        }
    }
}
