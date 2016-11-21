using System.Collections.Generic;
using LandisGyr.AMI.Devices.Capabilities.Processors;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using LandisGyr.AMI.Layers.DataContracts.ControlEvents.Device.Meter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    [TestClass]
    public class TestCapabilitiesMapper
    {
        [TestMethod]
        [Description(@"This method verifies the conversion of capability from the common TO's which will be shared between EDPL and Framework")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestCoversionOfCommonTOToLoadProfileCapability()
        {
            LoadProfile loadProfileTO =  new LoadProfile();
            loadProfileTO.IntervalLength = 15;
            loadProfileTO.MeterStorageCapacity = 200;
            loadProfileTO.Channels = GetChannels();

            LoadProfileCapabilityMapper mapper = new LoadProfileCapabilityMapper();
            LoadProfileCapability convertedCapability = mapper.GetCapabilityFromCommonTO(loadProfileTO);

            Assert.IsNotNull(convertedCapability);
            Assert.AreEqual(convertedCapability.Frequency, loadProfileTO.IntervalLength);
            Assert.AreEqual(convertedCapability.Capacity, loadProfileTO.MeterStorageCapacity);
            Assert.AreEqual(convertedCapability.Registers.Count, loadProfileTO.Channels.Count);

            foreach (Channel channel in loadProfileTO.Channels)
            {
                // Compare the registers of the common TO and coonverted capability.
                Assert.AreEqual(channel.ReadingType, convertedCapability.Registers[channel.ReadingType].Identifier);
            }
        }

        [TestMethod]
        [Description(@"This method verifies the conversion of capability from the common TO's which will be shared between EDPL and Framework")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DailySnap)]
        public void TestCoversionOfCommonTOToDailySnapCapability()
        {
            DailySnap dailySnap = new DailySnap();
            dailySnap.Frequency = 1440;
            dailySnap.MeterStorageCapacity = 30;
            dailySnap.Channels = GetChannels();

            DailySnapCapabilityMapper mapper = new DailySnapCapabilityMapper();
            DailySnapCapability convertedCapability = mapper.GetCapabilityFromCommonTO(dailySnap);

            Assert.IsNotNull(convertedCapability);
            Assert.AreEqual(convertedCapability.Frequency, dailySnap.Frequency);
            Assert.AreEqual(convertedCapability.Capacity, dailySnap.MeterStorageCapacity);
            Assert.AreEqual(convertedCapability.Registers.Count, dailySnap.Channels.Count);

            foreach (Channel channel in dailySnap.Channels)
            {
                // Compare the registers of the common TO and coonverted capability.
                Assert.AreEqual(channel.ReadingType, convertedCapability.Registers[channel.ReadingType].Identifier);
            }
        }

        [TestMethod]
        [Description(@"This method verifies the conversion of capability from the common TO's which will be shared between EDPL and Framework")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DemandReset)]
        public void TestCoversionOfCommonTOToDemandResetCapability()
        {
            Billing demandResetTO = new Billing();
            demandResetTO.Frequency = 15;
            demandResetTO.MeterStorageCapacity = 200;
            demandResetTO.Channels = GetChannels();

            DemandResetCapabilityMapper mapper = new DemandResetCapabilityMapper();
            DemandResetCapability convertedCapability = mapper.GetCapabilityFromCommonTO(demandResetTO);

            Assert.IsNotNull(convertedCapability);
            Assert.AreEqual(convertedCapability.Frequency, demandResetTO.Frequency);
            Assert.AreEqual(convertedCapability.Capacity, demandResetTO.MeterStorageCapacity);
            Assert.AreEqual(convertedCapability.SupportsRecursiveBillingDate, demandResetTO.SupportsRecursiveBillingDate);
            Assert.AreEqual(convertedCapability.SupportsMultipleBillingDates, demandResetTO.SupportsMultipleBillingDates);
            Assert.AreEqual(convertedCapability.Registers.Count, demandResetTO.Channels.Count);

            foreach (Channel channel in demandResetTO.Channels)
            {
                // Compare the registers of the common TO and coonverted capability.
                Assert.AreEqual(channel.ReadingType, convertedCapability.Registers[channel.ReadingType].Identifier);
            }
        }

        private List<Channel> GetChannels()
        {
            List<Channel> channels = new List<Channel>();

            Channel channel = new Channel();
            channel.ReadingType = "1.2.3.4.5.6.7.8.9";
            channels.Add(channel);

            channel = new Channel();
            channel.ReadingType = "1.2.3.4.5.6.7.8.10";
            channels.Add(channel);

            return channels;
        }
    }
}
