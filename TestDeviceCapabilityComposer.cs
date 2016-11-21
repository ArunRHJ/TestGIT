using System;
using System.Collections.Generic;
using System.Reflection;
using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.Devices;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceCapabilityComposer
    {
        /// <summary>
        /// This test method that when we try to fetch the capabilities of a group of a specified capability type from the cache for first time, 
        /// all the capability details get added in the cache, next time whenever we call the cache for the details of capability of a group, 
        /// they already exists in the cache and same will be returned.
        /// </summary>
        [TestMethod]
        [Description(@"This test method that when we try to fetch the capabilities of a group of a specified capability type from the cache for first time, 
                       all the capability details get added in the cache, next time whenever we call the cache for the details of capability of a group, 
                       they already exists in the cache and same will be returned.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Caching, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestDeviceCapabilitiesCaching()
        {
            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

            DeviceCapabilityCatalogue deviceCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            IDeviceModelCapabilityStore capabilityStore = new MockDeviceModelCapabilityStore();

            DeviceCapabilitiesLoader capabilityLoader = new DeviceCapabilitiesLoader(capabilityStore, deviceCatalogue);
            DeviceModelCapabilitiesLoader modelCapabilityLoader = new DeviceModelCapabilitiesLoader(capabilityStore, deviceCatalogue);

            DeviceCapabilityComposer capabilityComposer = new DeviceCapabilityComposer(capabilityLoader, modelCapabilityLoader);

            PrivateObject obj = new PrivateObject(capabilityComposer);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilityInstanceFromDataStore = obj.Invoke("GetCapabilityDetailsForGroup", bindingFlgs, Constants.DeviceModelGroupCrc, CC.CapabilityType.Registers);

            object capabilityInstanceFromCache = obj.Invoke("GetCapabilityDetailsForGroup", bindingFlgs, Constants.DeviceModelGroupCrc, CC.CapabilityType.Registers);

            Assert.AreEqual(capabilityInstanceFromCache, capabilityInstanceFromDataStore);
        }

        /// <summary>
        /// This test method verifies if a capability is supported for a combination of deviceModel, pduModel and commsTechModel Crcs
        /// If the capability is supported by even any one of the models , then it is considered to be supported for the device.
        /// </summary>
        [TestMethod]
        [Description(@"This test method verifies if a capability is supported for a combination of deviceModel, pduModel and commsTechModel Crcs
                        If the capability is supported by even any one of the models , then it is considered to be supported for the device.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Caching, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestIsCapabilitySupportedByDevice()
        {
            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

            DeviceCapabilityCatalogue deviceCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            IDeviceModelCapabilityStore capabilityStore = new MockDeviceModelCapabilityStore();

            DeviceCapabilitiesLoader capabilityLoader = new DeviceCapabilitiesLoader(capabilityStore, deviceCatalogue);
            DeviceModelCapabilitiesLoader modelCapabilityLoader = new DeviceModelCapabilitiesLoader(capabilityStore, deviceCatalogue);

            DeviceCapabilityComposer deviceCapabilityComposer = new DeviceCapabilityComposer(capabilityLoader, modelCapabilityLoader);

            bool isRegsitersCapabilitySupported = deviceCapabilityComposer.IsCapabilitySupportedByDevice(CC.CapabilityType.Registers, Constants.DeviceModelGroupCrc, Constants.PduModelGroupCrc,
                                                                    Constants.CommsTechModelGroupCrc, null);

            bool isCommandsCapabilitySupported = deviceCapabilityComposer.IsCapabilitySupportedByDevice(CC.CapabilityType.Commands, Constants.DeviceModelGroupCrc, Constants.PduModelGroupCrc,
                                                                    Constants.CommsTechModelGroupCrc, null);

            Assert.IsTrue(isRegsitersCapabilitySupported);
            Assert.IsFalse(isCommandsCapabilitySupported);
        }

        /// <summary>
        /// This sample is used to verify the merging of Capabilities of pdu model and comms tech model of a specified capability type.
        /// Let's assume we have 2 registers supported by pdu model and 2 registers supported by comms tech model and there are no device model capabilities 
        /// then for a device, only pdu and comms tech capabilities will be merged.  
        /// So the device will have in total 4 registers supported.
        /// </summary>
        [TestMethod]
        [Description(@"Merge the Capabilities of pdu model and comms tech model only  for a specified capability type, when device model capabilities are null")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestMergingOfPduAndCommsTechCapabilities()
        {
            int numOfPduRegisters = 0;
            int numofCommsTechRegisters = 0;

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

            DeviceCapabilityCatalogue deviceCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            IDeviceModelCapabilityStore capabilityStore = new MockDeviceModelCapabilityStore();
            DeviceCapabilitiesLoader capabilityLoader = new DeviceCapabilitiesLoader(capabilityStore, deviceCatalogue);
            DeviceModelCapabilitiesLoader modelCapabilityLoader = new DeviceModelCapabilitiesLoader(capabilityStore, deviceCatalogue);

            DeviceCapabilityComposer capabilityComposer = new DeviceCapabilityComposer(capabilityLoader, modelCapabilityLoader);

            MockRegistersCapability capabilityInstance;
            MockRegistersCapabilityHandler registersCapabilityHandler = new MockRegistersCapabilityHandler();

            #region Get Registers List for Device, PDU and Comms Tech Model

            // Load Registers for PDU Model
            capabilityInstance = registersCapabilityHandler.LoadCapability(Constants.PduModelCapabilitySystemIdentifier) as MockRegistersCapability;
            numOfPduRegisters = capabilityInstance.Registers.Count;

            // Load Registers for Comms Tech Model
            capabilityInstance = registersCapabilityHandler.LoadCapability(Constants.CommsTechModelCapabilitySystemIdentifier) as MockRegistersCapability;
            numofCommsTechRegisters = capabilityInstance.Registers.Count;

            #endregion

            // Get the Merged Capability for a combination of device model , pdu model and comms tech model crcs from cache
            CapabilityBase capabilityDetails = capabilityComposer.GetDeviceCapabilityDetails(CC.CapabilityType.Registers, null,
                Constants.PduModelGroupCrc, Constants.CommsTechModelGroupCrc, null);

            capabilityInstance = capabilityDetails as MockRegistersCapability;

            Assert.AreEqual((numOfPduRegisters + numofCommsTechRegisters), capabilityInstance.Registers.Count);
        }

        /// <summary>
        /// This sample is used to verify that merging of Capabilities will return only comms tech capabilities of a specified capability type 
        /// in the case when both pdu model and device model capabilities are null.
        /// </summary>
        [TestMethod]
        [Description(@"Merge of the Capabilities will return comms tech model capabilities only for a specified capability type, 
        when device model and pdu model capabilities are null")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others, TargetCapabilityType.None)]
        public void TestMergingOfCapabilities()
        {
            int numOfRegisters = 0;

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";

            DeviceCapabilityCatalogue deviceCatalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            IDeviceModelCapabilityStore capabilityStore = new MockDeviceModelCapabilityStore();
            DeviceCapabilitiesLoader capabilityLoader = new DeviceCapabilitiesLoader(capabilityStore, deviceCatalogue);
            DeviceModelCapabilitiesLoader modelCapabilityLoader = new DeviceModelCapabilitiesLoader(capabilityStore, deviceCatalogue);

            DeviceCapabilityComposer capabilityComposer = new DeviceCapabilityComposer(capabilityLoader, modelCapabilityLoader);

            MockRegistersCapability capabilityInstance;
            MockRegistersCapabilityHandler registersCapabilityHandler = new MockRegistersCapabilityHandler();

            #region Get Registers List for Device, PDU and Comms Tech Model

            // Load Registers for Comms Tech Model
            capabilityInstance = registersCapabilityHandler.LoadCapability(Constants.CommsTechModelCapabilitySystemIdentifier) as MockRegistersCapability;
            numOfRegisters = capabilityInstance.Registers.Count;

            #endregion

            // Get the Merged Capability for a combination of device model , pdu model and comms tech model crcs from cache
            CapabilityBase capabilityDetails = capabilityComposer.GetDeviceCapabilityDetails(CC.CapabilityType.Registers, null,
                null, Constants.CommsTechModelGroupCrc, null);

            capabilityInstance = capabilityDetails as MockRegistersCapability;

            Assert.AreEqual(numOfRegisters, capabilityInstance.Registers.Count);
        }
    }
}
