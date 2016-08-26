using Common;
using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;

namespace TestClient
{
    class Program
    {
        private static string universeSchedulerAddress = "";

        static void Main(string[] args)
        {
            CreateUniverse();
            PrintUniverses();

            StartUniverseEventStream();
            Console.ReadKey();
        }

        private static void StartUniverseEventStream()
        {
            var universeScheduler = ServiceProxy.Create<IUniverseScheduler>(new Uri(universeSchedulerAddress));
            universeScheduler.StartAsync();
        }

        private static void PrintUniverses()
        {
            var universeRegistryAddress = new Uri("fabric:/EoTPlatform/UniverseRegistry");
            var universeRegistry = ServiceProxy.Create<IUniverseRegistry>(universeRegistryAddress, new ServicePartitionKey(1L));

            var universes = universeRegistry.GetUniversesAsync().GetAwaiter().GetResult();

            foreach(var universe in universes)
            {
                Console.WriteLine($"Universe '{universe.Key}'");
                Console.WriteLine($"Id: {universe.Value.Id}");
                Console.WriteLine($"Status: {universe.Value.Status}");
                Console.WriteLine($"Services:");
                foreach(var service in universe.Value.ServiceEndpoints)
                {
                    Console.WriteLine($"{service.Key}: ");
                    foreach (var endpoint in service.Value)
                    {
                        Console.WriteLine($"\t{endpoint}");
                    }
                    
                }
                Console.WriteLine(new String('-', 10));
                Console.WriteLine(Environment.NewLine);
            }
        }

        public static bool CreateUniverse()
        {
            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeTemplateFilePath = $"{projectFolderPath}\\UniverseTemplate.json";
            string universeEventStreamFilePath = $"{projectFolderPath}\\UniverseData.csv";

            var universeSpecfication = new UniverseSpecification
            {
                Id = "0",
                UniverseTemplateFilePath = universeTemplateFilePath,
                UniverseEventStreamFilePath = universeEventStreamFilePath
            };

            var universeFactoryAddress = new Uri("fabric:/EoTPlatform/UniverseFactory");
            var universeFactory = ServiceProxy.Create<IUniverseFactory>(universeFactoryAddress);
            var universeDefinition = universeFactory.CreateUniverseAsync(universeSpecfication).GetAwaiter().GetResult();

            universeSchedulerAddress = universeDefinition.ServiceEndpoints["UniverseSchedulerType"][0];

            return true;
        }
    }
}
