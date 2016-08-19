using Common;
using Common.Models;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApplicationClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var success = CreateUniverse();

            if (success)
                Console.WriteLine("Success!");
            else
                Console.WriteLine("Failed!");

            Console.ReadKey();
        }

        public static bool CreateUniverse()
        {
            string currentFolderPath = Environment.CurrentDirectory;
            string projectFolderPath = currentFolderPath.Substring(0, currentFolderPath.IndexOf("bin"));
            string universeTemplateFilePath = $"{projectFolderPath}\\UniverseTemplate.json";

            var universeSpecfication = new UniverseSpecification
            {
                Id = "0",
                UniverseTemplateFilePath = universeTemplateFilePath
            };

            var universeFactoryAddress = new Uri("fabric:/EoTPlatform/UniverseFactory");
            var universeFactory = ServiceProxy.Create<IUniverseFactory>(universeFactoryAddress);
            var universeDescriptor = universeFactory.CreateUniverseAsync(universeSpecfication).GetAwaiter().GetResult();

            Console.WriteLine("----------------");
            Console.WriteLine("Created Universe");
            Console.WriteLine("----------------");
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Service Endpoints:");
            foreach (var endpoint in universeDescriptor.ServiceEndpoints)
            {
                Console.WriteLine(endpoint.Key + " : " + endpoint.Value);
            }
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("----------------");
            Console.WriteLine(Environment.NewLine);
            return true;
        }
    }
}
