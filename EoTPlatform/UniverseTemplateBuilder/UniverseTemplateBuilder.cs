using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using System.IO;
using Newtonsoft.Json;
using Common.Interfaces;
using Common.Models;

namespace UniverseTemplateBuilder
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class UniverseTemplateBuilder : StatelessService, IUniverseTemplateBuilder
    {
        public UniverseTemplateBuilder(StatelessServiceContext context)
            : base(context)
        { }

        public Task<string> BuildUniverseTemplateFromEndpointAsync(string sourceUrl)
        {
            throw new NotImplementedException();
        }

        public async Task BuildUniverseTemplateFromCSVAndWriteToJsonFileAsync(string inputFilePath, string outputFilePath)
        {
            var json = await BuildUniverseTemplateFromCSVAsync(inputFilePath);
            System.IO.File.WriteAllText(outputFilePath, json);
        }

        public async Task<string> BuildUniverseTemplateFromCSVAsync(string inputPath)
        {
            var actors = await GetActorsFromCSVFileAsync(inputPath);
            var actorTemplates = new List<ActorTemplate>();

            foreach (var actor in actors)
            {
                var template = new ActorTemplate
                {
                    Id = actor.Key,
                    Type = actor.Value
                };
                actorTemplates.Add(template);
            }

            var universeTemplate = new UniverseTemplate();
            universeTemplate.Id = new Random().Next(0, 1000).ToString();
            universeTemplate.ActorTemplates = actorTemplates;

            var json = JsonSerialiseUniverseTemplate(universeTemplate);

            return json;
        }

        private string JsonSerialiseUniverseTemplate(UniverseTemplate universeTemplate)
        {
            return JsonConvert.SerializeObject(universeTemplate);
        }

        private static async Task<Dictionary<string, string>> GetActorsFromCSVFileAsync(string filePath)
        {
            Dictionary<string, string> actors = new Dictionary<string, string>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    string line = "";
                    string[] values = null;
                    int i = 0;

                    while (!reader.EndOfStream)
                    {
                        line = await reader.ReadLineAsync();

                        // Assume headers
                        if (i == 0)
                        {
                            i++;
                            continue;
                        }

                        values = line.Split(',');

                        var id = values[0].Trim();

                        if (!actors.Keys.Contains(id))
                        {
                            // New actor
                            var type = values[1].Trim();
                            actors.Add(id, type);
                        }

                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return actors;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(context => this.CreateServiceRemotingListener(context))
            };
        }
    }
}
