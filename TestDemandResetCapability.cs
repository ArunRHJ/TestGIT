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
        [Description(@"This test method will verify handler's Type for a Demand Reset configured capability is same as default handler")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DemandReset)]
        public void TestDemandResetCapabilityHandlerType()
        {
            DemandResetCapabilityAbstractFactory demandResetAbstractFactory = new DemandResetCapabilityAbstractFactory();

            CapabilityHandler handler = demandResetAbstractFactory.CapabilityHandler;

            Assert.AreEqual(typeof(ProfileBasedCapabilityHandler<DemandResetCapability>), handler.GetType());
        }

        [TestMethod]
        [Description(@"This test method verifies the 'Creation' and 'Loading' of the same configured capability (Demand Reset in this case) in/from capabilities' data store.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DemandReset)]
        public void TestDemandResetConfiguredCapability()
        {
            // Clean the database
            DBHelper.CleanDatabase();

            string capabilityHash = "DRHash12345";
            int frequency = 43200;
            int capacity = 12;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            DemandResetCapability demandResetCapability = GetDemandResetCapabilityInstance(frequency, capacity, true, true, registers);

            DemandResetCapabilityAbstractFactory demandResetAbstractFactory = new DemandResetCapabilityAbstractFactory();
           
            #region Create Configured Capability

            // Create a configured capability - Demand Reset
            bool isCnfgrdCpbltyCreated = demandResetAbstractFactory.CapabilityHandler.CreateCapabilityIfNotExists(demandResetCapability, 
                CC.CapabilitySource.InitPush, capabilityHash);

            Assert.IsTrue(isCnfgrdCpbltyCreated);

            #endregion

            #region Load Configured Capability

            // Load the configured capability from the data store
            CapabilityBase capability = demandResetAbstractFactory.CapabilityHandler.LoadCapability(capabilityHash);

            Assert.IsNotNull(capability);

            DemandResetCapability loadedCapability = capability as DemandResetCapability;

            Assert.AreEqual(demandResetCapability.Frequency, loadedCapability.Frequency);
            Assert.AreEqual(demandResetCapability.Capacity, loadedCapability.Capacity);
            Assert.AreEqual(demandResetCapability.Registers.Count, loadedCapability.Registers.Count);

            foreach (KeyValuePair<string, Register> register in loadedCapability.Registers)
            {
                // Compare the register identifiers of the registers created and loaded back.
                Assert.AreEqual(register.Value.Identifier, demandResetCapability.Registers[register.Key].Identifier);
            }

            #endregion
        }

        [TestMethod]
        [Description(@"This test method is used to validate that Demand Reset capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.DemandReset)]
        public void ValidateDemandResetCapabilitySerialization()
        {
            int frequency = 43200;
            int capacity = 12;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            DemandResetCapability demandReset= GetDemandResetCapabilityInstance(frequency, capacity, true, true, registers);

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(demandReset.GetType());
            serializer.WriteObject(fs, demandReset);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            Assert.IsTrue(txt.Contains("</DemandResetCapability>"), "Required Demand Reset node missing");

            //Following Asserts verify that Properties of Demand Reset instance are serialized
            Assert.IsTrue(txt.Contains("<FrequencyForCrcComputer>"), "Required Frequency node of Demand Reset instance missing");
            Assert.IsTrue(txt.Contains("<CapacityForCrcComputer>"), "Required Capacity node of Demand Reset instance missing");
            Assert.IsTrue(txt.Contains("<SupportsMultipleBillingDatesForCrcComputer>"), "Required SupportsMultipleBillingDates node of Demand Reset instance missing");
            Assert.IsTrue(txt.Contains("<SupportsRecursiveBillingDateForCrcComputer>"), "Required SupportsRecursiveBillingDate node of Demand Reset instance missing");
            Assert.IsTrue(txt.Contains("<CapabilityIdentifierForCrcComputer>"), "Required CapabilityIdentifier node of Demand Reset instance missing");
        }

        /// <summary>
        /// Returns the instance of Demand Reset capability
        /// </summary>
        /// <returns></returns>
        private DemandResetCapability GetDemandResetCapabilityInstance(int frequency, int capacity, bool supportsMultipleBillingDates, bool supportsRecursiveBillingDate,
            List<string> registerIdentifiers)
        {
            Dictionary<string, Register> registers = new Dictionary<string, Register>();

            foreach (string registerIdentifier in registerIdentifiers)
            {
                Register register = new Register(registerIdentifier);
                registers.Add(register.Identifier, register);
            }

            DemandResetCapability demandResetCapability = new DemandResetCapability(frequency, capacity, supportsMultipleBillingDates, supportsRecursiveBillingDate, registers);
           
            return demandResetCapability;
        }
    }
}
