using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using System.Diagnostics.CodeAnalysis;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceModelCapabilitiesLoader
    {
        private string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

        [TestMethod]
        [Description(@"This test method is to check that capability is getting loaded properly with thier respective handler")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void ValidateModelCapabilties()
        {
            string modelName = "DummyModel";

            List<KeyValuePair<CC.CapabilityType, string>> cpbltyList = new List<KeyValuePair<CC.CapabilityType, string>>();
            cpbltyList.Add(new KeyValuePair<CC.CapabilityType, string> ( CC.CapabilityType.Registers, string.Empty ));

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            PrivateObject obj = new PrivateObject(typeof(DeviceModelCapabilitiesLoader), default(IDeviceModelCapabilityStore), catalogue);
            List<Tuple<string, CapabilityBase>> capabilities = (List<Tuple<string, CapabilityBase>>)(obj.Invoke("LoadModelCapabilities", new object[] { modelName, cpbltyList }));

            Assert.IsNotNull(capabilities, "Register Capability should have been supported");
            Assert.IsTrue(capabilities.Count >= 1);
            Assert.IsTrue(capabilities.Count(f => f.Item2.CapabilityType == CC.CapabilityType.Registers) == 1);
        }

        [TestMethod]
        [Description(@"This test method is to check that a capability supported by model failed to loaded by loaders")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        [ExcludeFromCodeCoverageAttribute]
        public void ValidateModelCapabiltiesNotLoaded()
        {
            string modelName = "DummyModel";
            bool isFailed = false;

            List<KeyValuePair<CC.CapabilityType, string>> cpbltyList = new List<KeyValuePair<CC.CapabilityType, string>>();
            //Capability whose Abstract factory is missing
            cpbltyList.Add(new KeyValuePair<CC.CapabilityType, string> (CC.CapabilityType.Events, string.Empty ));

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            PrivateObject obj = new PrivateObject(typeof(DeviceModelCapabilitiesLoader), default(IDeviceModelCapabilityStore), catalogue);
            try
            {
                obj.Invoke("LoadModelCapabilities", new object[] { modelName, cpbltyList });
            }
            catch (Exception ex)
            {
                isFailed = ex is ApplicationException;
            }

            Assert.IsTrue(isFailed, "Application Exception should occur because Events capability is not present");
        }

        #region Capability with Faulty handler

        [CapabilityAbstractFactoryAttribute(CC.CapabilityType.Commands)]
        [ExcludeFromCodeCoverageAttribute]
        private class DummyCommandsCapabilityAbstractFactory : CapabilityAbstractFactory
        {

            public override CapabilityBuilder CapabilityBuilder
            {
                get { throw new NotImplementedException(); }
            }

            public override CapabilityHandler CapabilityHandler
            {
                get { return new FaultyCapabilityHandler(); }
            }
        }

        [ExcludeFromCodeCoverageAttribute]
        private class FaultyCapabilityHandler : CapabilityHandler
        {
            public override bool CreateCapabilityIfNotExists(CapabilityBase capabilityDetails, Definitions.CapabilitySource source, string capabilityCrc)
            {
                throw new NotImplementedException();
            }

            public override CapabilityBase LoadCapability(string capabilityCrc)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
