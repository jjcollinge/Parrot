﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class UniverseTemplate
    {
        public string Id { get; set; }
        public List<ActorTemplate> ActorTemplates { get; set; }
    }
}