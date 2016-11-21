using System;
using System.Collections.Generic;
using System.Reflection;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public partial class TestCaching
    {
        /// <summary>
        /// This test method verifies that we are able to fetch the capability type details from cache properly ,while calling the cache for the first time the capability 
        /// type details do not exist in the cache therefore they are added to the cache
        /// Next time whenever we call cache for the same capability type, values are fetched from the cache and they are same as first one.
        /// </summary>
       [TestMethod]
        [Description(@"This test method verifies that we are able to fetch the capability type details from cache.
                      It will validate that when we try to fetch value for a capability type from the cache for first time, the value gets added in the cache
                      next time whenever we call the cache for the values, they already exists in the cache and same will be returned. Two capability type 
                      instance are considered equal if the two instances and all the elements for the capability type instances are equal 
                      If the capability type exists in the cache, the values are returned from the cache otherwise cache is updated for future reference.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Caching, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestCapabilityTypeCaching()
        {
            CapabilityInMemoryStore cache = new CapabilityInMemoryStore();

            string capabilitySystemIdentifier1st = "12345";
            MockRegistersCapability registersCapabilityInstance1 = new MockRegistersCapability(null);
            CapabilityBase cacheAccessOutput1 = cache.GetCapabilityTypeInstance(capabilitySystemIdentifier1st, CC.CapabilityType.Registers, registersCapabilityInstance1);

            string capabilitySystemIdentifier2nd = "12345";
            MockRegistersCapability registersCapabilityInstance2 = new MockRegistersCapability(null);
            CapabilityBase cacheAccessOutput2 = cache.GetCapabilityTypeInstance(capabilitySystemIdentifier2nd, CC.CapabilityType.Registers, registersCapabilityInstance2);
           
           // Two capability type instance are considered equal if the two instances and all the elements for the capability type instances are equal
            Assert.AreEqual(cacheAccessOutput2, cacheAccessOutput1);

            IElementsBasedCapability output1 = cacheAccessOutput1 as IElementsBasedCapability;
            IElementsBasedCapability output2 = cacheAccessOutput2 as IElementsBasedCapability;

            foreach (String key in output1.Elements.Keys)
            {
                Assert.AreEqual(output1.Elements[key], output2.Elements[key]);
            }
        }
    }
}
