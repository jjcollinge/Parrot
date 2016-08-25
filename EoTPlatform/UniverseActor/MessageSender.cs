﻿using Microsoft.Azure.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor
{
    public class MessageSender
    {
        private ServiceClient client;
        private string deviceId;

        public MessageSender(string deviceId, string connStr)
        {
            this.deviceId = deviceId;
            this.client = ServiceClient.CreateFromConnectionString(connStr);
        }

        /// <summary>
        /// Send a message to the cloud.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(byte[] msg)
        {
            await client.SendAsync(deviceId, new Message(msg));
        }
    }
}
