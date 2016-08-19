using Common;
using Common.Interfaces;
using Common.Models;
using Microsoft.ServiceFabric.Services.Client;
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

            if(success)
                success = ValidateUniverse();

            PrintResult(success);

            PrintNewLine();
            Console.ReadKey();
        }

        private static bool ValidateUniverse()
        {
            var universeRegistryAddress = new Uri("fabric:/EoTPlatform/UniverseRegistry");
            var universeRegistry = ServiceProxy.Create<IUniverseRegistry>(universeRegistryAddress, new ServicePartitionKey(1L));

            var universes = universeRegistry.GetUniversesAsync().GetAwaiter().GetResult();

            if (universes.Count == 1)
                return true;
            else
                return false;
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

            PrintHeader("Created Universe");
            PrintNewLine();
            PrintSubHeader("Service Endpoints");
            foreach (var service in universeDescriptor.ServiceEndpoints)
            {
                var endpoints = service.Value;
                Console.Write(service.Key + ": ");
                int i = 1;
                foreach (var endpoint in endpoints)
                {
                    var ep = endpoint;

                    if (endpoints.Count - i != 0)
                    {
                        ep += ", ";
                        i++;
                    }

                    Console.WriteLine(ep);
                }
            }
            PrintFooter("Created Universe");
            return true;
        }

        private static void PrintFooter(string text)
        {
            PrintNewLine();
            Console.WriteLine(new String('-', text.Count()));
            PrintNewLine();
        }

        private static void PrintSubHeader(string text)
        {
            Console.WriteLine($"{text}");
            PrintNewLine();
        }

        private static void PrintHeader(string text)
        {
            Console.WriteLine(new String('-', text.Count()));
            Console.WriteLine($"{text}");
            Console.WriteLine(new String('-', text.Count()));
            PrintNewLine();
        }

        private static void PrintNewLine()
        {
            Console.WriteLine(Environment.NewLine);
        }

        private static void PrintResult(bool success)
        {
            if(success)
                Console.WriteLine("SUCCESS!");
            else
                Console.WriteLine("FAIL!");
        }
    }
}
