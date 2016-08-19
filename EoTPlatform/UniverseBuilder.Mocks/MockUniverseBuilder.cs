﻿using Common.Interfaces;
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
        public async Task<UniverseDescriptor> BuildUniverseAsync(UniverseTemplate template)
        {
            var endpoint = new Uri("fabric:/mock/mock");
            return new UniverseDescriptor
            {
                ServiceEndpoints = new Dictionary<string, Uri> { { "mock", endpoint } }
            };
        }
    }
}
