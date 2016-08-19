using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseTemplateBuilder.Mocks
{
    public class MockUniverseTemplateBuilder : IUniverseTemplateBuilder
    {
        public Task<string> BuildUniverseTemplateFromEndpointAsync(string sourceUrl)
        {
            var json =  "{\"id\":\"0\",\"actorTemplates\":[{\"id\": \"0\",\"type\": \"train\",\"publishers\": [\"1\"]},{\"id\":\"1\",\"type\":\"trackSection\",\"publishers\":[]}]}";
            return Task.FromResult(json);
        }

        public Task<string> BuildUniverseTemplateFromCSVAsync(string inputFilePath)
        {
            var json = "{\"id\":\"0\",\"actorTemplates\":[{\"id\": \"0\",\"type\": \"train\",\"publishers\": [\"1\"]},{\"id\":\"1\",\"type\":\"trackSection\",\"publishers\":[]}]}";
            return Task.FromResult(json);
        }

        public Task BuildUniverseTemplateFromCSVAndWriteToJsonFileAsync(string inputFilePath, string outputFilePath)
        {
            return Task.FromResult(true);
        }
    }
}
