using System;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public partial class TestCaching
    {
        private class CapabilityTypeElement
        {
            public string Identifier { get; private set; }

            public CapabilityTypeElement(string identifier)
            {
                Identifier = identifier;
            }
        }

        /// <summary>
        /// This test method verifies that we are able to fetch the capability type elements from cache.
        /// It will validate that while calling the cache for the first time when capability type elements do not exist in the cache, they get added to the cache
        /// and next time whenever we call cache for the same capability type elements , values are fetched from the cache and they are same as first one.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies that we are able to fetch the capability type details from cache.
                      It validates that when we try to fetch value for a capability element for the first time from the cache, 
                      it is added in the cache and next time whenever we call the cache for the values, they already exists in the cache therefore the same will be returned.
                      If the capability type element exists in the cache, the values are returned from the cache otherwise cache is updated for future reference.
                       ")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Caching, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestCapabilityTypeElementsCaching()
        {
            CapabilityElementsInMemoryStore cache = new CapabilityElementsInMemoryStore();

            CapabilityTypeElement cpbltyElement1st = new CapabilityTypeElement("1.2.3.4.5.6.7.8.9");
            Object cacheAccessOutput1 = cache.GetCapabilityTypeElementInstance(CC.CapabilityType.Registers, cpbltyElement1st.Identifier, cpbltyElement1st);

            CapabilityTypeElement cpbltyElement2nd = new CapabilityTypeElement("1.2.3.4.5.6.7.8.9");
            Object cacheAccessOutput2 = cache.GetCapabilityTypeElementInstance(CC.CapabilityType.Registers, cpbltyElement2nd.Identifier, cpbltyElement2nd);

            Assert.AreNotEqual(cpbltyElement2nd, cacheAccessOutput2);
            Assert.AreEqual(cacheAccessOutput2, cacheAccessOutput1);
        }
    }
}
