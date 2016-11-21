using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using LandisGyr.AMI.Devices.Capabilities.CapabilityStoreImplementation;
using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LandisGyr.AMI.Layers.DataContracts.ControlEvents.Device.Meter;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilitiesProviderService
    {
        [TestMethod]
        [Description(@"This method verifies the fetching of model capabilities from EDPL layer using the call back URL , in case the model doesn't exists at EDS.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Model, TargetCapabilityType.None)]
        public void TestModelCapabilitiesProviderService()
        {
            DBHelper.CleanDatabase();

            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            IDeviceModelCapabilityStore modelCapabilityStore = new DeviceModelCapabilityStore();

            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(modelCapabilityStore, catalogue);

            string modelName = "DeviceModel";
            string callbackUrl = "http://localhost:63396/MockCapabilitiesProvider.svc";

            PrivateObject obj = new PrivateObject(capabilitiesProcessor, new PrivateType(typeof(CapabilitiesProcessorBase<MeterRegistrationInformation>)));
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("CreateNewModel", bindingFlgs, modelName, callbackUrl);

            List<Tuple<String, CapabilityBase>> capabilitiesList = capabilities as List<Tuple<String, CapabilityBase>>;

            #region Asserts

            Assert.IsNotNull(capabilitiesList);
         
            #endregion
        }

        [TestMethod]
        [Description(@"This method verifies that an exception is thrown while fetching the capabilities from EDPL if call back URL is either incorrect or inaccesible")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Model, TargetCapabilityType.None)]
        [ExcludeFromCodeCoverageAttribute]
        public void TestExceptionForInvalidCallBackURL()
        {
            DBHelper.CleanDatabase();

            bool isExceptionRaised = false;

            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(null, null);
             
            string modelName = "DeviceModel";
            string callbackUrl = "http://localhost:8080/MockCapabilitiesProvider.svc";

            PrivateObject obj = new PrivateObject(capabilitiesProcessor, new PrivateType(typeof(CapabilitiesProcessorBase<MeterRegistrationInformation>)));
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            try
            {
                object capabilities = obj.Invoke("CreateNewModel", bindingFlgs, modelName, callbackUrl);
            }
            catch
            {
                isExceptionRaised = true;
            }

            
            #region Asserts

            Assert.IsTrue(isExceptionRaised);

            #endregion
        }
    }
}
