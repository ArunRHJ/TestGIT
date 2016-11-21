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
    [TestClass]
    public  partial class TestConfiguredCapability
    {
        [TestMethod]
        [Description(@"This test method is used to check the defaults of the Load Profile Capability")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestDefaultValuesOfLoadProfileCapability()
        {
            CapabilityBase loadProfileCapability = new LoadProfileCapability();
            Assert.AreEqual(loadProfileCapability.CapabilityType, LandisGyr.AMI.Devices.Capabilities.Definitions.CapabilityType.LoadProfile);
            Assert.AreEqual(loadProfileCapability.IsConfigurationDependent, true);
        }

        [TestMethod]
        [Description(@"This test method will verify handler's Type for a Load Profile configured capability is same as default handler")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestLoadProfileCapabilityHandlerType()
        {
            LoadProfileCapabilityAbstractFactory loadProfileAbstractFactory = new LoadProfileCapabilityAbstractFactory();

            CapabilityHandler handler = loadProfileAbstractFactory.CapabilityHandler;

            Assert.AreEqual(typeof(ProfileBasedCapabilityHandler<LoadProfileCapability>), handler.GetType());
        }

        [TestMethod]
        [Description(@"This test method verifies the 'Creation' and 'Loading' of the same configured capability (LP in this case) in/from capabilities' data store.")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(2)]
        [EDSTestCategory(TargetTestType.Remote, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void TestLoadProfileConfiguredCapability()
        {
            // Clean the database
            DBHelper.CleanDatabase();

            string capabilityHash = "LPHash12345";
            int frequency = 15;
            int capacity = 200;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            LoadProfileCapability loadProfileCapability = GetLoadProfileCapabilityInstance(frequency, capacity, registers);

            LoadProfileCapabilityAbstractFactory loadProfileAbstractFactory = new LoadProfileCapabilityAbstractFactory();
           
            #region Create Configured Capability

            // Create a configured capability - Load Profile
            bool isCnfgrdCpbltyCreated = loadProfileAbstractFactory.CapabilityHandler.CreateCapabilityIfNotExists(loadProfileCapability, 
                CC.CapabilitySource.InitPush, capabilityHash);

            Assert.IsTrue(isCnfgrdCpbltyCreated);

            #endregion

            #region Load Configured Capability

            // Load the configured capability from the data store
            CapabilityBase capability = loadProfileAbstractFactory.CapabilityHandler.LoadCapability(capabilityHash);

            Assert.IsNotNull(capability);

            LoadProfileCapability loadedCapability = capability as LoadProfileCapability;

            Assert.AreEqual(loadProfileCapability.Frequency, loadedCapability.Frequency);
            Assert.AreEqual(loadProfileCapability.Capacity, loadedCapability.Capacity);
            Assert.AreEqual(loadProfileCapability.Registers.Count, loadedCapability.Registers.Count);

            foreach (KeyValuePair<string, Register> register in loadedCapability.Registers)
            {
                // Compare the register identifiers of the registers created and loaded back.
                Assert.AreEqual(register.Value.Identifier, loadProfileCapability.Registers[register.Key].Identifier);
            }

            #endregion
        }

        [TestMethod]
        [Description(@"This test method is used to validate that load profile capability is serialized as expected")]
        [Owner("email:platformTeam@Landisgyr.com")]
        [Priority(1)]
        [EDSTestCategory(TargetTestType.Unit, TargetFrameworkArea.Capabilities, TargetCapabilityCategory.Configured, TargetCapabilityType.LoadProfile)]
        public void ValidateLoadProfileCapabilitySerialization()
        {
            int frequency = 15;
            int capacity = 200;
            List<string> registers = new List<string> { "1.2.3.4.5.6.7.8.9" };

            LoadProfileCapability loadProfile = GetLoadProfileCapabilityInstance(frequency, capacity, registers);

            var fs = new MemoryStream();
            DataContractSerializer serializer = new DataContractSerializer(loadProfile.GetType());
            serializer.WriteObject(fs, loadProfile);
            fs.Close();

            string txt = Encoding.UTF8.GetString(fs.ToArray());

            Assert.IsTrue(txt.Contains("</LoadProfileCapability>"), "Required Load Profile node missing");

            //Following Asserts verify that Properties of Load Profile instance are serialized
            Assert.IsTrue(txt.Contains("<FrequencyForCrcComputer>"), "Required Frequency node of Load Profile instance missing");
            Assert.IsTrue(txt.Contains("<CapacityForCrcComputer>"), "Required Capacity node of Load Profile instance missing");
            Assert.IsTrue(txt.Contains("<IsFullRegisterReadForCrcComputer>"), "Required IsFullRegisterRead node of Load Profile instance missing");
            Assert.IsTrue(txt.Contains("<CapabilityIdentifierForCrcComputer>"), "Required CapabilityIdentifier node of Load Profile instance missing");
        }

        /// <summary>
        /// Returns the instance of load profile capability
        /// </summary>
        /// <returns></returns>
        private LoadProfileCapability GetLoadProfileCapabilityInstance(int frequency, int capacity, List<string> registerIdentifiers)
        {
            LoadProfileCapability loadProfileCapability = new LoadProfileCapability();
            loadProfileCapability.Frequency = frequency;
            loadProfileCapability.Capacity = capacity;

            Dictionary<string, Register> registers = new Dictionary<string, Register>();

            foreach (string registerIdentifier in registerIdentifiers)
            {
                Register register = new Register(registerIdentifier);
                registers.Add(register.Identifier, register);
            }

            loadProfileCapability.Registers = new ReadOnlyDictionary<string, Register>(registers);

            return loadProfileCapability;
        }
    }
}
