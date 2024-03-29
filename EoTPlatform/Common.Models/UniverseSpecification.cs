﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    [DataContract]
    public class UniverseSpecification
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string UniverseTemplateFilePath { get; set; }

        [DataMember]
        public string UniverseEventStreamFilePath { get; set; }
    }
}
