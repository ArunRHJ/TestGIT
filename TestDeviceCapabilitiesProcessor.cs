using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using LandisGyr.AMI.Devices.Capabilities.CapabilityStoreImplementation;
using LandisGyr.AMI.Devices.Capabilities.Definitions;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using LandisGyr.AMI.Layers.DataContracts.ControlEvents.Device.Meter;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestDeviceCapabilitiesProcessor
    {
        #region Test Methods

        [TestMethod]
        [Description(@"This test method verifies that load profile feature received in meter registration information from EDPL 
        is mapped to the correct capability type in the framework.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestMappingOfLoadProfileMeterFeatureToCapability()
        {
            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(null, catalogue);

            short intervalLength = 15;
            short capacity = 200;
            bool isFullRegRead = true;
            List<string> registers = new List<string>{"1.2.3.4.5.6.7.8.9"};

            LoadProfile loadProfileFeature = GetLoadProfileInstance(intervalLength, isFullRegRead, capacity, registers);

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("GetProcessedCapabilityFromMeterFeature", bindingFlgs, loadProfileFeature);

            ProcessedCapabilityDetails processedCapabilities = capabilities as ProcessedCapabilityDetails;

            #region Asserts

            Assert.AreEqual(DD.CapabilityType.LoadProfile, processedCapabilities.CapabilityType);
            #endregion
        }

        [TestMethod]
        [Description(@"This test method verifies that demand reset feature received in meter registration information from EDPL 
        is mapped to the correct capability type in the framework.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured, TargetCapabilityType.DemandReset)]
        public void TestMappingOfDemandResetMeterFeatureToCapability()
        {
            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(null, catalogue);

            int frequency = 43200;
            short capacity = 200;
            bool supportsMultipleBillingDates = true;
            bool supportsRecursiveBillingDates = true;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            Billing demandResetFeature = GetDemandResetInstance(frequency, supportsMultipleBillingDates, supportsRecursiveBillingDates, capacity, registers);

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("GetProcessedCapabilityFromMeterFeature", bindingFlgs, demandResetFeature);

            ProcessedCapabilityDetails processedCapabilities = capabilities as ProcessedCapabilityDetails;

            #region Asserts

            Assert.AreEqual(DD.CapabilityType.DemandReset, processedCapabilities.CapabilityType);
            #endregion
        }

        [TestMethod]
        [Description(@"This test method verifies that daily snap feature received in meter registration information from EDPL 
        is mapped to the correct capability type in the framework.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured, TargetCapabilityType.DailySnap)]
        public void TestMappingOfDailySnapMeterFeatureToCapability()
        {
            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(null, catalogue);

            int frequency = 43200;
            short capacity = 200;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            DailySnap dailySnapFeature = GetDailySnapInstance(frequency, capacity, registers);

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("GetProcessedCapabilityFromMeterFeature", bindingFlgs, dailySnapFeature);

            ProcessedCapabilityDetails processedCapabilities = capabilities as ProcessedCapabilityDetails;

            #region Asserts

            Assert.AreEqual(DD.CapabilityType.DailySnap, processedCapabilities.CapabilityType);
            #endregion
        }

        [TestMethod]
        [Description(@"This test method verifies the conversion of the meter features received in configuration information from EDPL 
        to procesesd configured capabilities")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured)]
        public void TestGetConfiguredCapabilitiesFromMeterConfiguration()
        {
            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";

            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);
            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(null, catalogue);

            MeterRegistrationInformation meterConfiguration = GetMeterRegistrationInformation();

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("GetConfigurationBasedCapabilities", bindingFlgs, meterConfiguration);

            List<ProcessedCapabilityDetails> processedCapabilities = capabilities as List<ProcessedCapabilityDetails>;

            #region Asserts

            Assert.AreEqual(meterConfiguration.Capabilities.Count, processedCapabilities.Count);
            #endregion
        }

        [TestMethod]
        [Description(@"This test method verifies the persistence of configured capabilities and configured capability group in data store")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Processors, TargetCapabilityCategory.Configured)]
        public void TestConfiguredCapabilitiesPersistence()
        {
            DBHelper.CleanDatabase();

            IDeviceModelCapabilityStore modelCapabilityStore = new DeviceModelCapabilityStore();
            IConfiguredCapabilityProfileStore configuredCapabilityStore = new ConfiguredCapabilityProfileStore();

            DeviceCapabilitiesProcessor capabilitiesProcessor = new DeviceCapabilitiesProcessor(modelCapabilityStore, null);

            string configuredCapabilitiesIdentifier = "CCG_12345";

            List<ProcessedCapabilityDetails> configuredCapabilities = new List<ProcessedCapabilityDetails>();
            ProcessedCapabilityDetails capability = new ProcessedCapabilityDetails();

            #region Create a configured capability Instance

            string deviceCataloguePath = @"..\..\..\..\LandisGyr.AMI.Devices.Capabilities.Implementation\bin\Debug";
            DeviceCapabilityCatalogue catalogue = new DeviceCapabilityCatalogue(deviceCataloguePath);

            capability.CapabilitySystemIdentifier = "LP_12345";
            capability.CapabilityType = DD.CapabilityType.LoadProfile;
            capability.Handler = catalogue[DD.CapabilityType.LoadProfile].CapabilityHandler;

            LoadProfileCapability lpCapability = new LoadProfileCapability();
            lpCapability.Frequency = 15;
            lpCapability.Capacity = 200;

            Dictionary<string, Register> registerDict = new Dictionary<string, Register>();
            registerDict.Add("1.2.3.4.5.6.7.8.9", new Register("1.2.3.4.5.6.7.8.9"));
            lpCapability.Registers = new ReadOnlyDictionary<string, Register>(registerDict);

            capability.Capability = lpCapability; 
            #endregion

            configuredCapabilities.Add(capability);

            PrivateObject obj = new PrivateObject(capabilitiesProcessor);
            BindingFlags bindingFlgs = BindingFlags.NonPublic | BindingFlags.Instance;

            object capabilities = obj.Invoke("PersistConfiguredCapabilities", bindingFlgs, configuredCapabilities, configuredCapabilitiesIdentifier);

            Assert.IsTrue(modelCapabilityStore.ConfiguredProfileSetExists("CCG_12345") > 0);
            Assert.IsTrue(configuredCapabilityStore.ProfileExists("LP_12345", (int)ProfileCode.LoadProfile));
        }

        #region Private Methods
        /// <summary>
        /// Returns the instance of meter registration information of a device have demand resret, load profile and daily snap capabilities
        /// </summary>
        /// <returns></returns>
        private MeterRegistrationInformation GetMeterRegistrationInformation()
        {
            MeterRegistrationInformation meterRegInfo = new MeterRegistrationInformation();
            meterRegInfo.Capabilities = new List<MeterFeature>();

            meterRegInfo.Capabilities.Add(GetLoadProfileInstance(15, true, 200, new List<string> { "1.2.3.4.5.6.7.8.9" }));

            meterRegInfo.Capabilities.Add(GetDemandResetInstance(43200, true, true, 12, new List<string> { "1.2.3.4.5.6.7.8.9" }));

            meterRegInfo.Capabilities.Add(GetDailySnapInstance(1440, 30, new List<string> { "1.2.3.4.5.6.7.8.9" }));

            return meterRegInfo;
        }

        /// <summary>
        /// This method return the instance of a meter feature of Load Profile type
        /// </summary>
        /// <param name="intervalLength">Interval length of Load profile</param>
        /// <param name="isFullRegRead">True if the LP is full register read otherwise false</param>
        /// <param name="capacity">Max Capacity of the device of recording LP</param>
        /// <param name="registers">List of resgisters read in LP</param>
        /// <returns>Instance of Load Profile Feature</returns>
        private LoadProfile GetLoadProfileInstance(short intervalLength, bool isFullRegRead, short capacity, List<string> registers)
        {
            LoadProfile loadProfileFeature = new LoadProfile();
            loadProfileFeature.IntervalLength = intervalLength;
            loadProfileFeature.IsFullRegReadFLG = isFullRegRead;
            loadProfileFeature.MeterStorageCapacity = capacity;
            loadProfileFeature.Name = MeterFeatureType.LoadProfile;
            loadProfileFeature.Channels = GetChannels(registers);

            return loadProfileFeature;
        }

        /// <summary>
        /// This method return the instance of a meter feature of Demand Reset type
        /// </summary>
        /// <param name="frequency">Frequency of occurence of Demand Reset</param>
        /// <param name="supportsMultipleBillingDates">True of device supports multiple billing date otherwise false</param>
        /// <param name="supportsRecursiveBillingDate">True of device supports recursive billing date otherwise false</param>
        /// <param name="capacity">Max Capacity of the device of recording demand reset data</param>
        /// <param name="registers">List of resgisters reset in demand reset</param>
        /// <returns>Instance of Demand Reset Feature</returns>
        private Billing GetDemandResetInstance(int frequency, bool supportsMultipleBillingDates, bool supportsRecursiveBillingDate,
            short capacity, List<string> registers)
        {
            Billing demandResetFeature = new Billing();
            demandResetFeature.Frequency = frequency;
            demandResetFeature.SupportsMultipleBillingDates = supportsMultipleBillingDates;
            demandResetFeature.SupportsRecursiveBillingDate = supportsRecursiveBillingDate;
            demandResetFeature.MeterStorageCapacity = capacity;
            demandResetFeature.Name = MeterFeatureType.Billing;
            demandResetFeature.Channels = GetChannels(registers);

            return demandResetFeature;
        }

        /// <summary>
        /// This method return the instance of a meter feature of Daily Snap type
        /// </summary>
        /// <param name="frequency">Frequency of occurence of Daily Snap<</param>
        /// <param name="capacity">Max Capacity of the device of recording daily snap</param>
        /// <param name="registers">List of resgisters read in daily snap</param>
        /// <returns>Instance of Daily snap Feature</returns>
        private DailySnap GetDailySnapInstance(int frequency, short capacity, List<string> registers)
        {
            DailySnap dailySnapFeature = new DailySnap();
            dailySnapFeature.Frequency = frequency;
            dailySnapFeature.MeterStorageCapacity = capacity;
            dailySnapFeature.Name = MeterFeatureType.DailySnap;
            dailySnapFeature.Channels = GetChannels(registers);

            return dailySnapFeature;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="registers"></param>
        /// <returns></returns>
        private List<Channel> GetChannels(List<string> registers)
        {
            List<Channel> channels = new List<Channel>();
            Channel channel;

            foreach (string register in registers)
            {
                channel = new Channel();
                channel.ReadingType = register;
                channels.Add(channel);
            }

            return channels;
        } 
        #endregion

        #endregion
    }
}



