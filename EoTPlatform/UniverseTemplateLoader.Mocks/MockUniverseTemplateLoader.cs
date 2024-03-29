﻿using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;

namespace UniverseTemplateLoader.Mocks
{
    public class MockUniverseTemplateLoader : IUniverseTemplateLoader
    {
        public Task<UniverseTemplate> LoadUniversalTemplateFromFileAsync(string templateFilePath)
        {
            return Task.FromResult(new UniverseTemplate
            {
                Id = "0",
                ActorTemplates = new List<ActorTemplate> { new ActorTemplate("0") }
            });
        }
    }
}
