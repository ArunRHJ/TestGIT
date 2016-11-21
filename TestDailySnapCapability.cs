using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using LandisGyr.AMI.Devices.Capabilities.TestLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC = LandisGyr.AMI.Devices.Capabilities.Definitions;

namespace LandisGyr.AMI.Devices.Capabilities.UnitTests
{
    public partial class TestConfiguredCapability
    {
        [TestMethod]
        [Description(@"This test method will verify handler's Type for a Daily Snap configured capability is same as default handler")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DailySnap)]
        public void TestDailySnapCapabilityHandlerType()
        {
            DailySnapCapabilityAbstractFactory dailySnapAbstractFactory = new DailySnapCapabilityAbstractFactory();

            CapabilityHandler handler = dailySnapAbstractFactory.CapabilityHandler;

            Assert.AreEqual(typeof(ProfileBasedCapabilityHandler<DailySnapCapability>), handler.GetType());
        }

        [TestMethod]
        [Description(@"This test method verifies the 'Creation' and 'Loading' of the same configured capability (DailySnap in this case) in/from capabilities' data store.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DailySnap)]
        public void TestDailySnapConfiguredCapability()
        {
            // Clean the database
            DBHelper.CleanDatabase();

            string capabilityHash = "DailySnapHash12345";
            int frequency = 1440;
            int capacity = 30;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            DailySnapCapability dailySnapCapability = GetDailySnapCapabilityInstance(frequency, capacity, registers);

            DailySnapCapabilityAbstractFactory dailySnapAbstractFactory = new DailySnapCapabilityAbstractFactory();

            #region Create Configured Capability

            // Create a configured capability - Daily Snap
            bool isCnfgrdCpbltyCreated = dailySnapAbstractFactory.CapabilityHandler.CreateCapabilityIfNotExists(dailySnapCapability, 
                CC.CapabilitySource.InitPush, capabilityHash);

            Assert.IsTrue(isCnfgrdCpbltyCreated);

            #endregion

            #region Load Configured Capability

            // Load the configured capability from the data store
            CapabilityBase capability = dailySnapAbstractFactory.CapabilityHandler.LoadCapability(capabilityHash);

            Assert.IsNotNull(capability);

            DailySnapCapability loadedCapability = capability as DailySnapCapability;

            Assert.AreEqual(dailySnapCapability.Frequency, loadedCapability.Frequency);
            Assert.AreEqual(dailySnapCapability.Capacity, loadedCapability.Capacity);
            Assert.AreEqual(dailySnapCapability.Registers.Count, loadedCapability.Registers.Count);

            foreach (KeyValuePair<string, Register> register in loadedCapability.Registers)
            {
                // Compare the register identifiers of the registers created and loaded back.
                Assert.AreEqual(register.Value.Identifier, dailySnapCapability.Registers[register.Key].Identifier);
            }

            #endregion
        }

        [TestMethod]
        [Description(@"This test method is used to validate that daily snap capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DailySnap)]
        public void ValidateDailySnapCapabilitySerialization()
        {
            int frequency = 1440;
            int capacity = 30;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            DailySnapCapability dailySnap = GetDailySnapCapabilityInstance(frequency, capacity, registers);

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(dailySnap.GetType());
            serializer.WriteObject(fs, dailySnap);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            Assert.IsTrue(txt.Contains("</DailySnapCapability>"), "Required Daily Snap node missing");

            //Following Asserts verify that Properties of Daily Snap instance are serialized
            Assert.IsTrue(txt.Contains("<FrequencyForCrcComputer>"), "Required Frequency node of Daily Snap instance missing");
            Assert.IsTrue(txt.Contains("<CapacityForCrcComputer>"), "Required Capacity node of Daily Snap instance missing");
            Assert.IsTrue(txt.Contains("<CapabilityIdentifierForCrcComputer>"), "Required CapabilityIdentifier node of Daily Snap instance missing");
        }


        /// <summary>
        /// Returns the instance of Daily Snap capability
        /// </summary>
        /// <returns></returns>
        private DailySnapCapability GetDailySnapCapabilityInstance(int frequency, int capacity, List<string> registerIdentifiers)
        {
            DailySnapCapability dailySnapCapability = new DailySnapCapability();
            dailySnapCapability.Frequency = frequency;
            dailySnapCapability.Capacity = capacity;

            Dictionary<string, Register> registers = new Dictionary<string, Register>();

            foreach (string registerIdentifier in registerIdentifiers)
            {
                Register register = new Register(registerIdentifier);
                registers.Add(register.Identifier, register);
            }

            dailySnapCapability.Registers = new ReadOnlyDictionary<string, Register>(registers);

            return dailySnapCapability;
        }
    }
}
