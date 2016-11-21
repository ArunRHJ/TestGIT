using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.DeviceCapabilityLoader;
using LandisGyr.AMI.Devices.Capabilities.Devices;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.Service;
using LandisGyr.AMI.Devices.Capabilities.Service.CapabilityMappers;
using LandisGyr.AMI.Devices.Capabilities.Service.Contracts;
using LandisGyr.AMI.Devices.Capabilities.Service.IoC;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using LandisGyr.IoC.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilityService
    {
        private IServiceLocator Locator { get; set; }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService returns Device capability as requested")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCapabiltyServiceBehaviour()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICapabilityMapper, MockCapabilityMapper>(CapabilityTypeTO.Registers.ToString());
            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            CapabilityTypeTO cpbltyType = CapabilityTypeTO.Registers;
            string deviceID = "12";
            CapabilityService service = new CapabilityService(Locator);
            DeviceTO mappedDevice = service.GetDeviceCapability(cpbltyType, deviceID);

            Assert.IsNotNull(mappedDevice, "CapabilityService did not returned Device Capabilities");
            Assert.IsNotNull(mappedDevice.Capabilities, "CapabilityService did not returned Device Capabilities");
            Assert.IsTrue(mappedDevice.DeviceIdentifier == deviceID, "Incorrect Device details fetched");
            Assert.IsTrue(mappedDevice.Capabilities.Count > 0, "CapabilityService did not returned required Capability");
            Assert.IsTrue(mappedDevice.Capabilities[0].CapabilityType ==  cpbltyType, string.Format("CapabilityService did not returned {0} Capability", cpbltyType.ToString()));
            Assert.IsNull(mappedDevice.DeviceDetails, "DeviceDetails is unexpected, it should be null");
        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService GetDeviceCapability operation behaviour when requested device not exists.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestGetDeviceCapabilityWhenRequestedDeviceNotExists()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICapabilityMapper, MockCapabilityMapper>(CapabilityTypeTO.Registers.ToString());
            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            CapabilityTypeTO cpbltyType = CapabilityTypeTO.Registers;
            
            string deviceID = Constants.DeviceIdForDeviceNotExist;

            CapabilityService service = new CapabilityService(Locator);
            DeviceTO mappedDevice = service.GetDeviceCapability(cpbltyType, deviceID);

            Assert.IsTrue(string.IsNullOrWhiteSpace(mappedDevice.DeviceIdentifier), "GetDeviceCapability operation return deviceIdentifier in DeviceTo when requested device not exists.");
            Assert.IsTrue(mappedDevice.HasFault, "GetDeviceCapability operation not setting fault in returned DeviceTo when requested device not exists.");
            Assert.IsNotNull(mappedDevice.ErrorInformation, "GetDeviceCapability operation is not providing errorInformation in returned DeviceTo when requested device not exists.");
            Assert.IsTrue(mappedDevice.ErrorInformation.Category == ErrorCategory.NotSupported, "GetDeviceCapability operation set different error category in returned DeviceTo when requested device not exists.");
            Assert.IsNull(mappedDevice.Capabilities, "GetDeviceCapability operation return capabilities in DeviceTo when requested device not exists.");
        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService GetDeviceCapability operation behaviour when requested Device having no capability.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestGetDeviceCapabilityWhenRequestedDeviceHaveNoCapability()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICapabilityMapper, MockCapabilityMapper>(CapabilityTypeTO.Registers.ToString());
            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            CapabilityTypeTO cpbltyType = CapabilityTypeTO.Registers;
            string deviceID = Constants.DeviceIdWithNoCapability;
            CapabilityService service = new CapabilityService(Locator);
            DeviceTO mappedDevice = service.GetDeviceCapability(cpbltyType, deviceID);

            Assert.IsFalse(string.IsNullOrWhiteSpace(mappedDevice.DeviceIdentifier), "GetDeviceCapability operation not returned deviceIdentifier in DeviceTo when requested device have no capabilites");
            Assert.IsTrue(mappedDevice.HasFault, "GetDeviceCapability operation not setting fault in returned DeviceTo when requested device have no capabilites.");
            Assert.IsNotNull(mappedDevice.ErrorInformation, "GetDeviceCapability operation is not providing errorInformation in returned DeviceTo when requested device have no capabilites.");
            Assert.IsTrue(mappedDevice.ErrorInformation.Category == ErrorCategory.NotSupported, "GetDeviceCapability operation set different error category in returned DeviceTo when requested device have no capabilites.");
            Assert.IsNull(mappedDevice.Capabilities, "GetDeviceCapability operation return capabilities in DeviceTo when requested device have no capabilites.");
        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService's GetDeviceAttribute operation returns Device attribute as requested")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCapabilityServiceGetDeviceAttributeOperation()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceModelCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            string assertMsg = "GetDeviceAttribute operation , {0} current value : {1} and expected value : {2} mismatch.";

            string deviceId = "12";

            string expectedDeviceIdentifier = deviceId;
            bool expectedIsBigEndian = true;
            string expectedDeviceType = DeviceType.ElectricMeter.ToString();
            bool expectedIsMultiUtilityCapable = true;
            bool expectedSupportReadingsParameterReprogramming = true;
            string expectedTargetMarket = TargetMarket.Residential.ToString();
            string expectedAddressingMechanism = AddressingMechanismTO.IP.ToString();
            bool expectedIsGridStream = true;
            bool expectedIsDSTCapable = true;

            CapabilityService service = new CapabilityService(Locator);
            DeviceTO deviceTO = service.GetDeviceAttributes(deviceId);

            string currentDeviceIdentifier = deviceTO.DeviceIdentifier;
            bool currentIsBigEndian = deviceTO.IsBigEndian;
            string currentDeviceType = deviceTO.DeviceType;
            bool currentIsMultiUtilityCapable = deviceTO.IsMultiUtilityCapable;
            bool currentSupportReadingsParameterReprogramming = deviceTO.SupportReadingsParameterReprogramming;
            string currentTargetMarket = deviceTO.TargetMarket.ToString();
            string currentAddressingMechanism = deviceTO.CommunicationTechnology.AddressingMechanism.ToString();
            bool currentIsGridStream = deviceTO.MessagingProtocol.IsGridStream;
            bool currentIsDSTCapable = deviceTO.IsDSTCapable;

            Assert.IsNotNull(deviceTO, "CapabilityService's GetDeviceAttribute operation did not return DeviceTo.");
            Assert.IsNull(deviceTO.Capabilities, "GetDeviceAttribute operation returned unexpected Device Capabilities.");

            Assert.IsTrue(currentDeviceIdentifier == expectedDeviceIdentifier, string.Format(assertMsg, "DeviceIdentifier", currentDeviceIdentifier, expectedDeviceIdentifier));
            Assert.IsTrue(currentIsBigEndian == expectedIsBigEndian, string.Format(assertMsg, "IsBigEndian", currentIsBigEndian.ToString(), expectedIsBigEndian.ToString()));
            Assert.IsTrue(currentDeviceType == expectedDeviceType, string.Format(assertMsg, "DeviceType", currentDeviceType, expectedDeviceType));
            Assert.IsTrue(currentIsMultiUtilityCapable == expectedIsMultiUtilityCapable, string.Format(assertMsg, "IsMultiUtilityCapable", currentIsMultiUtilityCapable, expectedIsMultiUtilityCapable));
            Assert.IsTrue(currentSupportReadingsParameterReprogramming == expectedSupportReadingsParameterReprogramming, string.Format(assertMsg, "SupportReadingsParameterReprogramming", currentSupportReadingsParameterReprogramming, expectedSupportReadingsParameterReprogramming));
            Assert.IsTrue(currentTargetMarket == expectedTargetMarket, string.Format(assertMsg, "TargetMarket", currentTargetMarket, expectedTargetMarket));
            Assert.IsTrue(currentAddressingMechanism == expectedAddressingMechanism, string.Format(assertMsg, "AddressingMechanism", currentAddressingMechanism, expectedAddressingMechanism));
            Assert.IsTrue(currentIsGridStream == expectedIsGridStream, string.Format(assertMsg, "IsGridStream", currentIsGridStream, expectedIsGridStream));
            Assert.IsTrue(currentIsDSTCapable == expectedIsDSTCapable, string.Format(assertMsg, "IsDSTCapable", currentIsDSTCapable, expectedIsDSTCapable));

        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService's GetDevicesAttribute operation returns Device attribute as requested")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCapabilityServiceGetDevicesAttributeOperation()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceModelCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            string[] deviceIds = { "12", "23", "34" };
            string assertMsg = "GetDeviceAttribute operation , {0} current value : {1} and expected value : {2} mismatch for Device {3}.";

            string expectedDeviceIdentifier;
            bool expectedIsBigEndian = true;
            string expectedDeviceType = DeviceType.ElectricMeter.ToString();
            bool expectedIsMultiUtilityCapable = true;
            bool expectedSupportReadingsParameterReprogramming = true;
            string expectedTargetMarket = TargetMarket.Residential.ToString();
            string expectedAddressingMechanism = AddressingMechanismTO.IP.ToString();
            bool expectedIsGridStream = true;
            bool expectedIsDSTCapable = true;

            CapabilityService service = new CapabilityService(Locator);
            DeviceTO[] deviceTOs = service.GetDevicesAttributes(deviceIds);
            DeviceTO deviceTO;

            string currentDeviceIdentifier;
            bool currentIsBigEndian;
            string currentDeviceType;
            bool currentIsMultiUtilityCapable;
            bool currentSupportReadingsParameterReprogramming;
            string currentTargetMarket;
            string currentAddressingMechanism;
            bool currentIsGridStream;
            bool currentIsDSTCapable;

            for (int counter = 0; counter < deviceIds.Length; counter++)
            {
                deviceTO = deviceTOs[counter];
                
                expectedDeviceIdentifier = deviceIds[counter];

                currentDeviceIdentifier = deviceTO.DeviceIdentifier;
                currentIsBigEndian = deviceTO.IsBigEndian;
                currentDeviceType = deviceTO.DeviceType;
                currentIsMultiUtilityCapable = deviceTO.IsMultiUtilityCapable;
                currentSupportReadingsParameterReprogramming = deviceTO.SupportReadingsParameterReprogramming;
                currentTargetMarket = deviceTO.TargetMarket.ToString();
                currentAddressingMechanism = deviceTO.CommunicationTechnology.AddressingMechanism.ToString();
                currentIsGridStream = deviceTO.MessagingProtocol.IsGridStream;
                currentIsDSTCapable = deviceTO.IsDSTCapable;

                Assert.IsNotNull(deviceTO, "CapabilityService's GetDeviceAttribute operation did not return DeviceTo.");
                Assert.IsNull(deviceTO.Capabilities, "GetDeviceAttribute operation returned unexpected Device Capabilities");


                Assert.IsTrue(currentDeviceIdentifier == expectedDeviceIdentifier, string.Format(assertMsg, "DeviceIdentifier", currentDeviceIdentifier, expectedDeviceIdentifier, currentDeviceIdentifier));
                Assert.IsTrue(currentIsBigEndian == expectedIsBigEndian, string.Format(assertMsg, "IsBigEndian", currentIsBigEndian.ToString(), expectedIsBigEndian.ToString(), currentDeviceIdentifier));
                Assert.IsTrue(currentDeviceType == expectedDeviceType, string.Format(assertMsg, "DeviceType", currentDeviceType, expectedDeviceType, currentDeviceIdentifier));
                Assert.IsTrue(currentIsMultiUtilityCapable == expectedIsMultiUtilityCapable, string.Format(assertMsg, "IsMultiUtilityCapable", currentIsMultiUtilityCapable, expectedIsMultiUtilityCapable, currentDeviceIdentifier));
                Assert.IsTrue(currentSupportReadingsParameterReprogramming == expectedSupportReadingsParameterReprogramming, string.Format(assertMsg, "SupportReadingsParameterReprogramming", currentSupportReadingsParameterReprogramming, expectedSupportReadingsParameterReprogramming, currentDeviceIdentifier));
                Assert.IsTrue(currentTargetMarket == expectedTargetMarket, string.Format(assertMsg, "TargetMarket", currentTargetMarket, expectedTargetMarket, currentDeviceIdentifier));
                Assert.IsTrue(currentAddressingMechanism == expectedAddressingMechanism, string.Format(assertMsg, "AddressingMechanism", currentAddressingMechanism, expectedAddressingMechanism, currentDeviceIdentifier));
                Assert.IsTrue(currentIsGridStream == expectedIsGridStream, string.Format(assertMsg, "IsGridStream", currentIsGridStream, expectedIsGridStream, currentDeviceIdentifier));
                Assert.IsTrue(currentIsDSTCapable == expectedIsDSTCapable, string.Format(assertMsg, "IsDSTCapable", currentIsDSTCapable, expectedIsDSTCapable, currentDeviceIdentifier));
            }

        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityService's GetDeviceAttribute operation behaviour when requested device not exists.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestGetDeviceAttributeOperationWhenRequestedDeviceNotExists()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceModelCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            string deviceId = Constants.DeviceIdForDeviceNotExist;
            CapabilityService service = new CapabilityService(Locator);

            DeviceTO mappedDevice = service.GetDeviceAttributes(deviceId);

            Assert.IsTrue(string.IsNullOrWhiteSpace(mappedDevice.DeviceIdentifier), "GetDeviceAttributes operation return deviceIdentifier in DeviceTo when requested device not exists.");
            Assert.IsTrue(mappedDevice.HasFault, "GetDeviceAttributes operation not setting fault in returned DeviceTo when requested device not exists.");
            Assert.IsNotNull(mappedDevice.ErrorInformation, "GetDeviceAttributes operation is not providing errorInformation in returned DeviceTo when requested device not exists.");
            Assert.IsTrue(mappedDevice.ErrorInformation.Category == ErrorCategory.NotSupported, "GetDeviceAttributes operation set different error category in returned DeviceTo when requested device not exists.");
            Assert.IsNull(mappedDevice.Capabilities, "GetDeviceAttributes operation return capabilities in DeviceTo when requested device not exists.");
        }
                                                   
        [TestMethod]
        [Description(@"This method is to check the CapabilityService's GetDevicesAttribute operation behaviour when requested device not exists.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestGetDevicesAttributeOperationWhenRequestedDeviceNotExists()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceModelCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            string[] deviceIds = { "12", "23", "34", Constants.DeviceIdForDeviceNotExist, "45", Constants.DeviceIdForDeviceNotExist };
            string assertMsg = "GetDeviceAttribute operation , {0} current value : {1} and expected value : {2} mismatch for Device {3}.";

            string expectedDeviceIdentifier;
            bool expectedIsBigEndian = true;
            string expectedDeviceType = DeviceType.ElectricMeter.ToString();
            bool expectedIsMultiUtilityCapable = true;
            bool expectedSupportReadingsParameterReprogramming = true;
            string expectedTargetMarket = TargetMarket.Residential.ToString();
            string expectedAddressingMechanism = AddressingMechanismTO.IP.ToString();
            bool expectedIsGridStream = true;
            bool expectedIsDSTCapable = true;

            CapabilityService service = new CapabilityService(Locator);
            DeviceTO[] deviceTOs = service.GetDevicesAttributes(deviceIds);
            DeviceTO deviceTO;

            string currentDeviceIdentifier;
            bool currentIsBigEndian;
            string currentDeviceType;
            bool currentIsMultiUtilityCapable;
            bool currentSupportReadingsParameterReprogramming;
            string currentTargetMarket;
            string currentAddressingMechanism;
            bool currentIsGridStream;
            bool currentIsDSTCapable;

            for (int counter = 0; counter < deviceIds.Length; counter++)
            {
                expectedDeviceIdentifier = deviceIds[counter];
                deviceTO = deviceTOs[counter];

                if (expectedDeviceIdentifier == Constants.DeviceIdForDeviceNotExist)
                {
                    Assert.IsTrue(string.IsNullOrWhiteSpace(deviceTO.DeviceIdentifier), "GetDeviceAttributes operation return deviceIdentifier in DeviceTo when requested device not exists.");
                    Assert.IsTrue(deviceTO.HasFault, "GetDeviceAttributes operation not setting fault in returned DeviceTo when requested device not exists.");
                    Assert.IsNotNull(deviceTO.ErrorInformation, "GetDeviceAttributes operation is not providing errorInformation in returned DeviceTo when requested device not exists.");
                    Assert.IsTrue(deviceTO.ErrorInformation.Category == ErrorCategory.NotSupported, "GetDeviceAttributes operation set different error category in returned DeviceTo when requested device not exists.");
                    Assert.IsNull(deviceTO.Capabilities, "GetDeviceAttributes operation return capabilities in DeviceTo when requested device not exists.");
                }
                else
                {
                    currentDeviceIdentifier = deviceTO.DeviceIdentifier;
                    currentIsBigEndian = deviceTO.IsBigEndian;
                    currentDeviceType = deviceTO.DeviceType;
                    currentIsMultiUtilityCapable = deviceTO.IsMultiUtilityCapable;
                    currentSupportReadingsParameterReprogramming = deviceTO.SupportReadingsParameterReprogramming;
                    currentTargetMarket = deviceTO.TargetMarket.ToString();
                    currentAddressingMechanism = deviceTO.CommunicationTechnology.AddressingMechanism.ToString();
                    currentIsGridStream = deviceTO.MessagingProtocol.IsGridStream;
                    currentIsDSTCapable = deviceTO.IsDSTCapable;

                    Assert.IsNotNull(deviceTO, "CapabilityService's GetDeviceAttribute operation did not return DeviceTo.");
                    Assert.IsNull(deviceTO.Capabilities, "GetDeviceAttribute operation returned unexpected Device Capabilities");


                    Assert.IsTrue(currentDeviceIdentifier == expectedDeviceIdentifier, string.Format(assertMsg, "DeviceIdentifier", currentDeviceIdentifier, expectedDeviceIdentifier, currentDeviceIdentifier));
                    Assert.IsTrue(currentIsBigEndian == expectedIsBigEndian, string.Format(assertMsg, "IsBigEndian", currentIsBigEndian.ToString(), expectedIsBigEndian.ToString(), currentDeviceIdentifier));
                    Assert.IsTrue(currentDeviceType == expectedDeviceType, string.Format(assertMsg, "DeviceType", currentDeviceType, expectedDeviceType, currentDeviceIdentifier));
                    Assert.IsTrue(currentIsMultiUtilityCapable == expectedIsMultiUtilityCapable, string.Format(assertMsg, "IsMultiUtilityCapable", currentIsMultiUtilityCapable, expectedIsMultiUtilityCapable, currentDeviceIdentifier));
                    Assert.IsTrue(currentSupportReadingsParameterReprogramming == expectedSupportReadingsParameterReprogramming, string.Format(assertMsg, "SupportReadingsParameterReprogramming", currentSupportReadingsParameterReprogramming, expectedSupportReadingsParameterReprogramming, currentDeviceIdentifier));
                    Assert.IsTrue(currentTargetMarket == expectedTargetMarket, string.Format(assertMsg, "TargetMarket", currentTargetMarket, expectedTargetMarket, currentDeviceIdentifier));
                    Assert.IsTrue(currentAddressingMechanism == expectedAddressingMechanism, string.Format(assertMsg, "AddressingMechanism", currentAddressingMechanism, expectedAddressingMechanism, currentDeviceIdentifier));
                    Assert.IsTrue(currentIsGridStream == expectedIsGridStream, string.Format(assertMsg, "IsGridStream", currentIsGridStream, expectedIsGridStream, currentDeviceIdentifier));
                    Assert.IsTrue(currentIsDSTCapable == expectedIsDSTCapable, string.Format(assertMsg, "IsDSTCapable", currentIsDSTCapable, expectedIsDSTCapable, currentDeviceIdentifier));
                }                 
            }     
        }

        [TestMethod]
        [Description(@"This method is to check CapabilityService output when there is a fault at service")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCapabiltyServiceExceptionBehaviour()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<ICapabilityMapper, MockCapabilityMapper>(CapabilityTypeTO.Registers.ToString());
            container.RegisterType<DeviceCapabilitiesLoader>();
            container.RegisterType<DeviceCapabilityComposer>();

            string deviceCataloguePath = @"..\..\..\LandisGyr.AMI.Devices.Capabilities.TestLibrary.Common\bin\Debug";
            container.RegisterType<IDeviceModelCapabilityStore, MockDeviceModelCapabilityStore>();
            container.RegisterInstance<DeviceCapabilityCatalogue>(new DeviceCapabilityCatalogue(deviceCataloguePath), new ContainerControlledLifetimeManager()); //Added syste.componentmodel.composition for this

            Locator = new UnityServiceLocator(container);
            container.RegisterInstance<IServiceLocator>(Locator, new ContainerControlledLifetimeManager());

            CapabilityTypeTO cpbltyType = CapabilityTypeTO.Registers;
            List<string> deviceIDs = new List<string>() {"120", "121"};

            CapabilityService service = new CapabilityService(Locator);
            DeviceTO[] mappedDevice = service.GetDevicesCapability(cpbltyType, deviceIDs.ToArray());

            Assert.IsNotNull(mappedDevice, "CapabilityService did not returned Device Capabilities");
            Assert.IsTrue(mappedDevice.Length == deviceIDs.Count, "Some Devices for which Capabilities requested are not returned. Error at Service");

            //Configured Mock store, assumimg last item does not exist in store.
            DeviceTO deviceWithFault = mappedDevice[deviceIDs.Count - 1];
            Assert.IsTrue(deviceWithFault.HasFault, "Flag should be set to true for device with Fault");
            Assert.IsNotNull(deviceWithFault.ErrorInformation, "ErrorInformation, should be set for the device with fault");
        }

        [TestMethod]
        [Description(@"This method is to check the CapabilityServiceBotStrapper has all required dependencies")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Others)]
        public void TestCapabilityServiceBootStrapper()
        {
            IUnityContainer container = new UnityContainer();
            IServiceLocator locator = new UnityServiceLocator(container);

            CapabilityServiceBootStrapperManager cpbltySvcBootStrapperMgr = new CapabilityServiceBootStrapperManager(container, locator);

            cpbltySvcBootStrapperMgr.RegisterServices();

            BootStrapperManager bootStrapperMgr = container.Resolve<BootStrapperManager>();
            IDeviceModelCapabilityStore dvcModelCpbltyStore = container.Resolve<IDeviceModelCapabilityStore>();
            DeviceCapabilitiesLoader deviceCapabilitiesLoader = container.Resolve<DeviceCapabilitiesLoader>();
            DeviceCapabilityComposer deviceCapabilityComposer = container.Resolve<DeviceCapabilityComposer>();
            DeviceCapabilitiesProcessor deviceCapabilitiesProcessor = container.Resolve<DeviceCapabilitiesProcessor>();
            DeviceCapabilityCatalogue deviceCapabilityCatalogue = container.Resolve<DeviceCapabilityCatalogue>();
            ICapabilityMapper registersetCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.Registers.ToString());
            ICapabilityMapper commandSetCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.Commands.ToString());
            ICapabilityMapper  eventSetCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.Events.ToString());
            ICapabilityMapper loadControlSwitchCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.LoadControlStatus.ToString());
            ICapabilityMapper dailySnapCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.DailySnap.ToString());
            ICapabilityMapper loadProfileCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.LoadProfile.ToString());
            ICapabilityMapper demandResetCapabilityMapper = container.Resolve<ICapabilityMapper>(CapabilityTypeTO.DemandReset.ToString());

            Assert.IsNotNull(bootStrapperMgr, "BootStrapperManager Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(dvcModelCpbltyStore, "DeviceModelCapabilityStore Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(deviceCapabilitiesLoader, "DeviceCapabilitiesLoader Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(deviceCapabilityComposer, "DeviceCapabilityComposer Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(deviceCapabilityCatalogue, "DeviceCapabilityCatalogue Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(registersetCapabilityMapper, "RegistersetCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(commandSetCapabilityMapper, "CommandSetCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(eventSetCapabilityMapper, "EventSetCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(loadControlSwitchCapabilityMapper, "LoadControlSwitchCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(dailySnapCapabilityMapper, "DailySnapCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(loadProfileCapabilityMapper, "LoadProfileCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
            Assert.IsNotNull(demandResetCapabilityMapper, "DemandResetCapabilityMapper Instance should be registered in CapabilityServiceBootStrapperManager");
        }
    }
}
