using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniverseActor.Tests
{
    class TestCallingDynamicallyGeneratedCustomerMethods
    {
        [TestMethod]
        public async Task TestCompileAssemblyFromSource()
        {
            var actor = new UniverseActor();

            await actor.ReceiveMessageAsync();
        }
    }
}
