﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public enum UniverseStatus
    {
        Building,
        Running,
        Failed
    }

    [DataContract]
    public class UniverseDefinition
    {
        public UniverseDefinition()
        {
            this.ServiceEndpoints = new Dictionary<string, List<string>>();
            this.Id = Guid.NewGuid().ToString();
            this.Status = UniverseStatus.Building;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public UniverseStatus Status { get; set; }        

        [DataMember]
        public Dictionary<string, List<string>> ServiceEndpoints { get; private set; }

        public void AddServiceEndpoints(string serviceName, List<string> serviceEndpoints)
        {
            this.ServiceEndpoints.Add(serviceName, serviceEndpoints);
        }
    }
}
