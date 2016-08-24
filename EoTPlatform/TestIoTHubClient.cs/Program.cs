using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestIoTHubClient.cs
{
    class Program
    {
        static ServiceClient serviceClient;
        static string connectionString = "HostName=eotiothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=Q156BMJEwL2Eg7vr/dMoa7hXmOB/b/rrEri6rHFJvaM=";

        static void Main(string[] args)
        {
            Console.WriteLine("Send Cloud-to-Device message\n");
            serviceClient = ServiceClient.CreateFromConnectionString(connectionString);

            Console.WriteLine("Press any key to send a C2D message.");
            Console.ReadLine();
            SendCloudToDeviceMessageAsync().Wait();
            Console.ReadLine();
        }

        private async static Task SendCloudToDeviceMessageAsync()
        {
            var commandMessage = new Message(Encoding.ASCII.GetBytes("Cloud to device message."));
            await serviceClient.SendAsync("myFirstDevice", commandMessage);
        }
    }
}
