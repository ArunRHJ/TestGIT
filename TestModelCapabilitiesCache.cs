using System;
using System.Collections.Generic;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public partial class TestCaching
    {
        /// <summary>
        /// This test method verifies that we are able to fetch the capability details of a model from cache properly ,while calling the cache for the first time the  
        /// capability details of a model do not exist in the cache therefore they are added to the cache
        /// Next time whenever we call cache for the same model, details are fetched from the cache and they are same as first one.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies that we are able to fetch the capability details of a model from cache.
                      It will validate that when we try to fetch the capabilities of a model from the cache for first time, they added in the cache
                      next time whenever we call the cache for the details of a model, they already exists in the cache and same will be returned.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Caching, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestModelCapabilitiesCaching()
        {
            IReadOnlyCollection<Tuple<String, CapabilityBase>> modelCapabilitiesInstance1 = null;
            IReadOnlyCollection<Tuple<String, CapabilityBase>> modelCapabilitiesInstance2 = null;

            ModelCapabilitiesInMemoryStore modelCache = new ModelCapabilitiesInMemoryStore();

            string modelNameInstance1 = "TEPCO_6N_200";
            List<Tuple<String, CapabilityBase>> capabilitiesInstance1 = new List<Tuple<string, CapabilityBase>>();
            MockRegistersCapability registersCapabilityInstance1 = new MockRegistersCapability(null);
            CapabilityBase registerCapabilityInstance1 = new MockRegistersCapability(null);
            capabilitiesInstance1.Add(new Tuple<String, CapabilityBase>("12345", registersCapabilityInstance1));
            modelCapabilitiesInstance1 = modelCache.GetModelCapabilities(modelNameInstance1, capabilitiesInstance1);

            string modelNameInstance2 = "TEPCO_6N_200";
            bool isModelExistsInCache = modelCache.IfModelExistsInCache(modelNameInstance2);
            if (isModelExistsInCache)
            {
                modelCapabilitiesInstance2 = modelCache.GetModelCapabilities(modelNameInstance2);
            }

            Assert.IsTrue(isModelExistsInCache);
            Assert.IsNotNull(modelCapabilitiesInstance2);
            Assert.AreEqual(modelCapabilitiesInstance2, modelCapabilitiesInstance1);
        }
    }
}
