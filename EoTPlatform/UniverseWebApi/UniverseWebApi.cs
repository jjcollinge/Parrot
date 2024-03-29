﻿using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using UniverseWebApi.Services;

namespace UniverseWebApi
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class UniverseWebApi : StatelessService
    {
        public UniverseWebApi(StatelessServiceContext context, IUniverseManagementService managementService)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[]
            {
                new ServiceInstanceListener(serviceContext => new OwinCommunicationListener(Startup.ConfigureApp, serviceContext, ServiceEventSource.Current, "ServiceEndpoint"))
            };
        }
    }
}
