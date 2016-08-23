using Common.Models;
using Common.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using UniverseWebApi.Model;
using UniverseWebApi.Services;

namespace UniverseWebApi.Controllers
{
    public class UniverseController : ApiController
    {
        // GET api/universe 
        public async Task<Dictionary<string, UniverseDescriptor>> GetAsync()
        {
            // Return a list of all the universe 
            var universeManagementService = new UniverseManagementService(new ServiceProxyFactory());
            var uninverse = await universeManagementService.GetUniverseDescriptorsAsync();
            return uninverse;
        }

        // GET api/universe/5 
        public async Task<UniverseDescriptor> GetAsync(int id)
        {
            var universeManagementService = new UniverseManagementService(new ServiceProxyFactory());
            var universe = await universeManagementService.GetUniverseDescriptorAsync(id.ToString());
            return universe;
        }

        // POST api/universe 
        public async Task PostAsync(UniversePayload payload)
        {
            var templateFilePath = payload.TemplateFilePath;
            var universeManagementService = new UniverseManagementService(new ServiceProxyFactory());
            await universeManagementService.CreateUniverseAsync(templateFilePath);
        }

        // DELETE api/universe/5 
        public async Task DeleteAsync(int id)
        {
            var universeManagementService = new UniverseManagementService(new ServiceProxyFactory());
            await universeManagementService.DeleteUniverseAsync(id.ToString());
        }
    }
}
