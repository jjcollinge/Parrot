using System;
using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using System.IO;
using Newtonsoft.Json;
using Common.Interfaces;
using Common.Models;
using Common.Services;

namespace UniverseTemplateLoader
{
    public sealed class UniverseTemplateLoader : StatelessService, IUniverseTemplateLoader
    {
        public UniverseTemplateLoader(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<UniverseTemplate> LoadUniversalTemplateFromFileAsync(string templateFilePath)
        {
            //TODO: Add further validation to filepath
            if (templateFilePath == null)
                return null;

            var ext = Path.GetExtension(templateFilePath);

            //TODO: Change to factory pattern and add support for more file types
            UniverseTemplate template = null;

            switch(ext.ToLowerInvariant())
            {
                case ".json":
                    template = await LoadJsonTemplateAsync(templateFilePath);
                    break;
                default:
                    throw new IOException("Unsupported file type");
            }

            return template;
        }

        /// <summary>
        /// Loads a universal template from a json file.
        /// </summary>
        /// <param name="templateFilePath"></param>
        /// <returns></returns>
        private async Task<UniverseTemplate> LoadJsonTemplateAsync(string templateFilePath)
        {
            var universeTemplate = new UniverseTemplate();

            try
            {
                using (StreamReader reader = new StreamReader(templateFilePath))
                {
                    string json = reader.ReadToEnd();

                    // TODO: Sort deserialization out
                    IDictionary<string, object> jsonDictionary = await Task.Run(() => JsonConvert.DeserializeObject<IDictionary<string, object>>(json, new JsonConverter[] { new UniverseTemplateConverter() }));
                    foreach (var universeKey in jsonDictionary.Keys)
                    {
                        var value = jsonDictionary[universeKey];
                        if (universeKey == "id")
                        {
                            universeTemplate.Id = value.ToString();
                        }
                        else if (universeKey == "version")
                        {
                            universeTemplate.Version = value.ToString();
                        }
                        else if (universeKey == "actorTemplates")
                        {
                            List<Dictionary<string, object>> actorTemplates = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(value.ToString());

                            foreach (var actorTemplate in actorTemplates)
                            {
                                var id = (string)actorTemplate["id"];
                                var actor = new ActorTemplate(id);
                                actor.Metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(actorTemplate["metadata"].ToString());
                                actor.Properties = JsonConvert.DeserializeObject<Dictionary<string, ActorTemplateProperty>>(actorTemplate["properties"].ToString());
                                actor.Commands = JsonConvert.DeserializeObject<List<string>>(actorTemplate["commands"].ToString());
                                universeTemplate.ActorTemplates.Add(actor);
                            }
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid file format");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ServiceEventSource.Current.ServiceMessage(this, $"Failed to load universe template file due to expception {ex}");
                throw ex;
            }

            return universeTemplate;
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
