using Common.Services.Models;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services.Interfaces
{
    public interface IUniverseBuilder : IService
    {
        Task<UniverseDescriptor> BuildUniverseAsync(UniverseTemplate template);
    }
}
