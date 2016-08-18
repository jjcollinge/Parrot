using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Common.Services.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Common.Services.Models;
using System.IO;
using Newtonsoft.Json;

namespace UniverseTemplateLoader
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    public sealed class UniverseTemplateLoader : StatelessService, IUniverseTemplateLoader
    {
        public UniverseTemplateLoader(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<UniverseTemplate> LoadTemplateFromFileAsync(string templateFilePath)
        {
            //TODO: Add further validation to filepath
            if (templateFilePath == null)
                return null;

            var ext = Path.GetExtension(templateFilePath);

            //TODO: Change to factory pattern and add support for more file types
            UniverseTemplate template = null;

            if (ext.ToLowerInvariant() == ".json")
            {
                template = await LoadJsonTemplateAsync(templateFilePath);
            }

            return template;
        }

        private async Task<UniverseTemplate> LoadJsonTemplateAsync(string templateFilePath)
        {
            UniverseTemplate template = null;

            try
            {
                using (StreamReader reader = new StreamReader(templateFilePath))
                {
                    string json = reader.ReadToEnd();
                    template = await Task.Run(() => JsonConvert.DeserializeObject<UniverseTemplate>(json));
                }
            }
            catch (Exception ex)
            {
                throw ex;
                //TODO: Handle exceptions
            }
            
            return template;
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
